import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FileUploadService } from '../services/file-upload.service';

@Component({
  selector: 'app-face-reader',
  templateUrl: './face-reader.component.html',
  styleUrls: ['./face-reader.component.css']
})
export class FaceReaderComponent implements OnInit {
    private destroy$: Subject<boolean> = new Subject<boolean>();
    // Variable to store shortLink from api response
    shortLink: string = "";
    loading: boolean = false; // Flag variable
    file: any; // Variable to store file
    name: string="";
    errorMessage: any;
    
    public nameForm:FormGroup;
    categoryName: string = "";
    // Inject service 
    constructor(private fileUploadService: FileUploadService,private formBuilder: FormBuilder) 
    {
      this.nameForm = this.formBuilder.group({
        name: ''
      });    
    }
  
    ngOnInit(): void {
    }
  
    // On file Select
    onChange(event:any) {
        this.file = event.target.files[0];
    }
  
    // OnClick of button Upload
    onUpload() {
        this.loading = !this.loading;
        console.log(this.file);
        this.categoryName=this.nameForm.get('name')?.value;
        this.fileUploadService.upload(this.file, this.categoryName)
        .pipe(takeUntil(this.destroy$))
        .subscribe(
          (data:any) => {
                   // Short link via api response
                   this.shortLink = data;
                   this.loading = false; // Flag variable
            },
          (error) => {
            this.errorMessage = 'Error while uploading the file';
          }
        );
    }

    // OnClick of button Identification
    onIdentify() {
      this.loading = !this.loading;
      console.log(this.file);
      this.fileUploadService.identify(this.file)
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        (data:any) => {
                 // Short link via api response
                 this.shortLink = data;
                 this.loading = false; // Flag variable
          },
        (error) => {
          this.errorMessage = 'Error while identify the source image';
        }
      );
  }

  onDelete() {
    this.loading = !this.loading;
    debugger
    this.fileUploadService.deletePersonGroup()
    .pipe(takeUntil(this.destroy$))
    .subscribe(
      (data:any) => {
               // Short link via api response
               debugger
               this.shortLink = data;
               this.loading = false; // Flag variable
        },
      (error) => {
        this.errorMessage = 'Error while deleteing person group ';
      }
    );
}


  }
