## Launching the Project in Visual Studio 2022

- Launch Visual Studio 2022
- Open .sln file
- In the **Solution Explorer** Select TestApplication.Server project as Startup project
- Press F5

## Validation
Currently relying on ASP.NET's built-in model binding to return 400 Bad Request for malformed JSON. Schema validation can be added later for stricter rules if needed.


# Part 2 
## LARGE ATTACHMENTS
## Data Structures

``` sql

CREATE TABLE Submissions (
    Id UUID PRIMARY KEY,
    FormData TEXT NOT NULL,
    SubmittedAt DATETIME NOT NULL
);

-- Attachments metadata
CREATE TABLE Attachments (
    Id UUID PRIMARY KEY,
    SubmissionId UUID NOT NULL REFERENCES Submissions(Id),
    FileName TEXT NOT NULL,
    ContentType TEXT NOT NULL,
    SizeBytes BIGINT NOT NULL,
    StoragePath TEXT NOT NULL,
    UploadedAt DATETIME NOT NULL
);
```
1. **FormData**: Store JSON as string or JsonDocument.

2. **StoragePath**: Local file path or S3/Blob URI.

## Storage Architecture

- **Cloud Storage for Large Files**: Since the attachments can be large, 
it’s ideal to use scalable and cost-effective cloud storage solutions like Amazon S3, 
Google Cloud Storage, or Azure Blob Storage. 
These platforms are optimized for storing large files, support high availability, 
and allow for easy retrieval.
- **Storage Strategy**:
    - Each submission (form entry) will have a metadata record in a database (relational or NoSQL).
    - The attachments themselves are stored in cloud storage, with each file assigned a unique identifier (e.g., a UUID or GUID)
    - Cloud storage provides benefits such as automatic scaling, redundancy, and secure access controls.
    - A file organization strategy can be used where files are stored in directories by submission ID or user ID (e.g., submissions/{submission_id}/attachment1, submissions/{submission_id}/attachment2, etc.)
- **CDN**: We can use a CDN to cache and serve attachments efficiently, especially when dealing with large files. This improves download speeds and reduces latency.

## Handling Scalability / High Traffic

- **Load Balancing**: Use load balancers to distribute requests evenly across application instances.

- **Caching**: Cache frequently requested metadata (e.g., submission details, attachment counts) using systems like Redis to reduce database load.

- **Asynchronous Processing**: File uploads and processing should be done asynchronously, especially for large attachments. We can use queues (e.g., RabbitMQ, Kafka, AWS SQS) to process file uploads and background tasks like virus scanning or file format conversions.

- **File Chunking**: For very large files (e.g., >100MB), we can consider uploading in chunks. This prevents timeout issues and helps resume uploads if they fail in between.


## Security and Access Control

- **Authentication**: Secure API access with OAuth or JWT for user authentication and authorization. Ensure that users can only access their submissions and attachments.

- **Data Encryption**: Use SSL/TLS for data in transit. Ensure files are encrypted at rest in the cloud storage.

- **Access Policies**: Use permissions for file access. For example, attachments can be private by default and only accessible through the API using presigned URLs.
- **File Type & Size Validation**: Validate file types and ensure size restrictions are set to prevent excessively large uploads that could overload your system
- Ensure files are scanned for malware before saving them to cloud storage

## Monitoring and Logging

- **File Upload Logging**: Log file uploads, downloads, and access attempts to monitor for potential issues (e.g., failed uploads, unauthorized access).

- **Error Handling**: Return appropriate HTTP status codes for failures (e.g., 400 for bad requests, 404 for not found, 500 for server errors).



## File Upload and Download API methods

```csharp
[HttpPost("submit")]
[RequestSizeLimit(200_000_000)] // Optional: Limit file size (200MB)
public async Task<IActionResult> SubmitForm()
{
    // Generate unique submission ID
    var submissionId = Guid.NewGuid();

    // Read the form data
    var form = await Request.ReadFormAsync();
    var formJson = form["form"];  // 'form' contains the JSON data from frontend

    // Parse the form data
    var formData = JsonNode.Parse(formJson);

    // Save submission record (storing the form data)
    var submission = new Submission
    {
        Id = submissionId,
        FormData = formData, // Store the parsed form data
        SubmittedAt = DateTime.UtcNow
    };
    db.Submissions.Add(submission);

    // Process each file in the 'files' field
    foreach (var file in form.Files)
    {
        var filePath = Path.Combine("Uploads", Guid.NewGuid().ToString());

        // Create file directory if necessary
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        // Save the file to disk
        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await file.CopyToAsync(fileStream);

        // Save file metadata to the database (Attachment)
        db.Attachments.Add(new Attachment
        {
            Id = Guid.NewGuid(),
            SubmissionId = submissionId,
            FileName = file.FileName,
            ContentType = file.ContentType,
            StoragePath = filePath,
            UploadedAt = DateTime.UtcNow,
            SizeBytes = file.Length
        });
    }

    // Commit the transaction (both Submission and Attachments)
    await db.SaveChangesAsync();

    return Ok(new { submissionId });
}
```





### Download Method

```csharp
[HttpGet("download/{submissionId}/{attachmentId}")]
public async Task<IActionResult> DownloadFile(Guid submissionId, Guid attachmentId)
{
    // Retrieve attachment metadata from the database
    var attachment = await db.Attachments
        .FirstOrDefaultAsync(a => a.Id == attachmentId && a.SubmissionId == submissionId);

    if (attachment == null)
    {
        return NotFound("Attachment not found.");
    }

    // Check if the file exists on disk
    if (!System.IO.File.Exists(attachment.StoragePath))
    {
        return NotFound("File not found on disk.");
    }

    // Use 'using' to ensure the stream is disposed of properly
    using var fileStream = new FileStream(attachment.StoragePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 8192, useAsync: true);

    // Return the file as a stream, letting ASP.NET Core handle the response headers for download
    return File(fileStream, attachment.ContentType, attachment.FileName);
}
```

