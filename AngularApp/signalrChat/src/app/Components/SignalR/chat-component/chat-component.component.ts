import { Component } from '@angular/core';
import { HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'app-chat-component',
  templateUrl: './chat-component.component.html',
  styleUrls: ['./chat-component.component.css']
})
export class ChatComponentComponent {
  private connection: any;

  public userName: string = '';
  public connectionId: string = '';
  public connectionState: number=0;
  public errorMessage: string = '';
  
  public connectedUsers: string[] = [];
  public messages: string[] = [];
  public newMessage: string = '';
  public messageDestination : string = '';
  ngOnInit() {
    
  }

  private connectToSignalR() {
    this.connectionState=1;

    this.connection = new HubConnectionBuilder()
      .withUrl('https://localhost:7055/ApplicationHub')
      .build();

    this.connection.start()
      .then(() => {
        this.connection.invoke('JoinApp', this.userName)
        .then()
        .catch((error: any) => {
          this.connectionState=3;
        });
        this.registerMessageHandlers();
      })
      .catch((error: any) => {
        console.error('Error establishing SignalR connection:', error);
        this.connectionState=3;
      });
  }

  private registerMessageHandlers() {
    
    this.connection.on('ConfirmJoin', (conId: string) => {
      this.connectionId=conId;
      this.connectionState=2;
    });

    this.connection.on('receiveMessage', (message: string) => {
      this.messages.push(message);
    });

    this.connection.on('userJoined', (names: string[]) => {
      this.connectedUsers= names
    });

    this.connection.on('userLeft', (name: string) => {
      const index = this.connectedUsers.indexOf(name);
      if (index !== -1) {
        this.connectedUsers.splice(index, 1);
      }
    });
  }

  public connectToHub(){
    if(this.userName){
      this.errorMessage = "";
      this.connectToSignalR();
    }else{
      this.errorMessage = "UserName is required to start a connection.";
    }
  }
  public sendMessage() {
    if (this.newMessage.trim() === '') return;

    this.connection.invoke('sendMessage', this.messageDestination, this.newMessage)
      .then(() => {
        this.newMessage = '';
      })
      .catch((error: any) => {
        console.error('Error sending message:', error);
      });
  }
}
