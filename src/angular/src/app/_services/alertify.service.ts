import { Injectable } from '@angular/core';

declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

    constructor() { }  
    // This is our dialog box that says like 'Are you sure you want to do this ?'
    // This is just a wrapper
    // This will take a message as the first parameters and the second parameter
    // is the okCallBack which is a function of type any that will be executed
    // when the user clicks the OK button
    confirm(message: string, okCallBack: () => any) {
      alertify.confirm(message, function (e) {
        // e here is the OK button that the user clicked
        if (e) {
          okCallBack();
        } else {} // this is empty since we don't need to do anything if the user clicks cancel
      });
    }
    success(message: string) {
      alertify.success(message);
    }
    error(message: string) {
      alertify.error(message);
    }  
    warning(message: string) {
      alertify.warning(message);
    }
    message(message: string) {
      alertify.message(message);
    }
  }
