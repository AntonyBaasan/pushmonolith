import { PutObjectCommand, CreateBucketCommand, S3Client } from "@aws-sdk/client-s3";
import * as fs from 'fs';


export class S3Service {

    public s3Client: S3Client;

    constructor() {
        this.s3Client = new S3Client({ region: 'us-east-1' });
    }

    public async createBucket(fileName: string): Promise<any> {
        const fileContent = fs.readFileSync(fileName);

        const params = {
            Bucket: "pushmonolith-2", // The name of the bucket. For example, 'sample_bucket_101'.
            Key: "app.jar", // The name of the object. For example, 'app.jar'.
            Body: fileContent, // The content of the object. For example, 'Hello world!".
        };

        try {
            const data = await this.s3Client.send(
                new CreateBucketCommand({ Bucket: params.Bucket })
            );
            console.log(data);
            console.log("Successfully created a bucket called ", data.Location);

        } catch (err) {
            console.log("Error", err);
        }

        // Create an object and upload it to the Amazon S3 bucket.
        try {
            const results = await this.s3Client.send(new PutObjectCommand(params));
            console.log(
                "Successfully created " + params.Key + " and uploaded it to " + params.Bucket + "/" + params.Key
            );
            return results; // For unit tests.
        } catch (err) {
            console.log("Error", err);
        }
    }

}