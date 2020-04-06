import { AlertifyService } from './../../_services/alertify.service';
import { AuthService } from './../../_services/auth.service';
import { UserService } from './../../_services/user.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {

  @Input() recipientId: number;

  newMessage: any = {};

  constructor(private userService: UserService, private authService: AuthService,
    private alertify: AlertifyService) { }

  ngOnInit() {
  }
}
