import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
    
  // API url
  baseApiUrl = "https://localhost:5001/"
   //https://localhost:5001/ 
  constructor(private http:HttpClient) { }
  
  // Returns an observable
  upload(file:any, employeeName:string):Observable<any> {
  
      // Create form data
      const formData = new FormData(); 
        
      // Store form name as "file" with file data
      formData.append("fileImage", file);
      formData.append("fileName", file.name);
      formData.append("employeeName",employeeName);
      //debugger
      // Make http post request over api
      // with formData as req
      //return this.http.post(this.baseApiUrl+"UploadFile/UploadBlobFile", formData)
      return this.http.post<string>(this.baseApiUrl+"TrainEmployeeFaceDetection/TrainEmployeeEngine",formData,{ responseType: 'text' as 'json' })
        .pipe(
            tap(data => console.log('All: '+data)),
            catchError(this.handleError)            
          );    
  }
  // Returns an observable
  identify(file:any):Observable<any> {
  
    // Create form data
    const formData = new FormData(); 
      
    // Store form name as "file" with file data
    formData.append("fileImage", file);
    formData.append("fileName", file.name);
    debugger
    // Make http post request over api
    // with formData as req
    //return this.http.post(this.baseApiUrl+"UploadFile/UploadBlobFile", formData)
    return this.http.post<string>(this.baseApiUrl+"TrainEmployeeFaceDetection/IdentifyEmployeeImage",formData,{ responseType: 'text' as 'json' })
      .pipe(
          tap(data => console.log('All: '+data)),
          catchError(this.handleError)            
        );    
}

deletePersonGroup():Observable<any> {
  
  // Make http post request over api
  // with formData as req
  //return this.http.post(this.baseApiUrl+"UploadFile/UploadBlobFile", formData)
  return this.http.delete<string>(this.baseApiUrl+"TrainEmployeeFaceDetection/DeletePersonGroup")
    .pipe(
        tap(data => console.log('All: '+data)),
        catchError(this.handleError)            
      );    
}

  private handleError(err: HttpErrorResponse) {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
   debugger
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }

}