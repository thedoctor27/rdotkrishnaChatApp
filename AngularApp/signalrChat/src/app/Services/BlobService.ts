import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BlobService {
  private baseUrl = 'https://localhost:7055/api/Blob'; // Replace with your ASP.NET API base URL

  constructor(private http: HttpClient) {}

  getAllBlobNames(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}`);
  }
  
  // Function to upload a blob to the server
  uploadBlob(userName: string,file: File): Observable<any> {
    const formData: FormData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post<any>(`${this.baseUrl}/${userName}`, formData);
  }

  // Function to download a blob from the server
  downloadBlob(blobName: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/${blobName}`, { responseType: 'blob' });
  }

  // Function to delete a blob on the server
  deleteBlob(userName: string,blobName: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/${userName}/${blobName}`);
  }
}
