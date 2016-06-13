# SRChat
SignalR based chat client for windows
______________________________________________________________________________________________________________________________________


This is a brief guide as to the processes used to create the SIgnalR WPF Chat application.

The basics are covered in this guide here: http://www.codeproject.com/Articles/804770/Implementing-SignalR-in-Desktop-Applications.  This covers setting up a chat hub, and setting up a basic desktop app to perform a chat.

Users: users are set in this application to be equal to their connection, in the sense of the fact that each person logged in from that connection (as in the same user with the application opened multiple times) is grouped into on user group; this group contains all of the duplicate logons.  

To share messages: The default option is to share with everyone.  To change this, simply send the id of the user that you wish to send the message to.  This can be obtained by using one of the methods that I created for this, such as getconnectionID.

Chatrooms: the default login state is entering into the global chatroom.  To send a private message, a user will send the message in question to everyone in the chatroom corresponding to the id sent to the program.  This will then alert the recipients to the message by flashing the PM notification, and appending PM to each received private message in the global chat.  These PMs will show up in both the private chat and global chat windows.  Additional chatroom support other than 1 to 1 PM messages is under development, but not yet finished.  The framework for setting this up is in place, just finish the add to group method and add the user to the group in question for the chatroom. An example of a working solution can be found here: http://www.codeproject.com/Articles/562023/Asp-Net-SignalR-Chat-Room, however this does not account for the fact that the users are already in groups pertaining to the consolidation of duplicate logins.  This would have to be set as a sub group within another group.

UI: the entire UI is built on top of C# and XAML.  This is complemented by the Material Design theme which makes the application fall into Google’s design standards and my personal preference in the modern design era (similar to MSFTs metro design, but more colorful and with added depth).  The program is built around tabs which allow for multiple windows within one main window.  These tabs can be dragged around and rearranged like chromes tabs.  They can also expand to create their own window using Draggablez.  For the tabs to work properly, the main program is set to have a static variable pointing to itself, that way when other windows try to access the data from it, they access the data from the current instance instead of creating a new one.

For future note: If in the future this applications feature set is to be expanded, have a look at Jabbr, and other open source clients based off of signalR, as most of these have more features, and are easier to use and learn.




