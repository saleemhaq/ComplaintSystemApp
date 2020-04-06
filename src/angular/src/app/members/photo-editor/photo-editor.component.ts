import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../_services/auth.service';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';

// This will be a child component of our member-edit.component.ts
// And on of the things that we will be bringing in from our member-edit.component.ts
// is a 'User' and the 'User' has an array of photos[].
@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Output() getMemberPhotoChange = new EventEmitter<string>();

  // Copied from https://valor-software.com/ng2-file-upload/
  uploader: FileUploader;
  hasBaseDropZoneOver = false;

  baseUrl = environment.apiUrl;

  constructor(private authService: AuthService, private userService: UserService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      // This is pointing to the API \DatingApp.API\Controllers\PhotosController.cs
      // where the route specified is 'api/users/{userId}/photos'
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      // We also need to authenticate so we need to pass the auth token
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      // After the photo has been successfully uploaded, we want to remove it from
      // the upload queue
      removeAfterUpload: true,
      // We want our users to click the button before we will upload
      autoUpload: false,
      // Setting the max file size to 10MB
      maxFileSize: 10 * 1024 * 1024
    });

    // Fix the ng2-file-uploader issue: Fail to load 'http://localhost:500/api/user/1/photos:
    // Response to preflight request doesn't pass access control check: The value of the
    // Access-Control-Allow-Origin header in the response must not be a the wildcard '*'
    // when the request credential mode is 'include'.  Origin 'http://localhost:4200' is
    // therefore not allowed access. The credentials mode of requests initiated by the XMLHttpRequest
    // is controlled by the withCredentials attribute.
    // We are going to extend our ng2-file-uploader that our file is not going with
    // credentials
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
  }
}
