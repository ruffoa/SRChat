using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNet.SignalR.Client;
using System.Net.Http;
using System.ComponentModel;
using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.IO;// Needed for Streaming...

namespace SRChat_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public static MainWindow m_Singleton = new SRChat_WPF.MainWindow();  // neccesary for accessing methods and data from the other tabs

        private NotifyData m_data;  // local variable used for updating various things
        private NotifyData u_data;

        HubConnection hubConnection;
        IHubProxy hubProxy;
        public const string ServerUrl = "http://localhost:49361/";
        public HubConnection Connection { get; set; }

        [Category("Data"), DescriptionAttribute("Gets/sets the connected user name.")]
        [Bindable(true)]
        public string ConnectedUser { get; set; }
        public string PrivateChat { get; set; }  // the chat string sent during a PM

        [Category("Data"), DescriptionAttribute("Gets/Sets the SignalR server hub address.")]
        [Bindable(true)]
        public string ServerURI { get; set; }

        public string names1 { get; set; }
        public string names2 { get; set; }
        public string chatnames { get; set; }

        private TaskbarIcon tb;
        public static Boolean finished { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ServerURI = "http://localhost:49361/";
            ConnectedUser = "WPF-Client-Beta";  // hardcoded for now - can change dynamically later
            PrivateChat = "";
            Task T = ConnectAsync();
            DataContext = this;
            m_data = new NotifyData();
            this.DataContext = m_data;  // This is the glue that connects the label to the object instance
            names1 = "";
            names2 = "";
            chatnames = "";
            txtnotify.Foreground = new SolidColorBrush(Colors.Black);  // change the color of the notification text
            u_data = new NotifyData();
            this.DataContext = u_data;  // This is the glue that connects the

            //DataContainer.notifications = true;
            tb = (TaskbarIcon) FindResource("MyNotifyIcon");  // icon for the taskbar notification
        }

        private static string myValue;

        public static string MyValue
        {
            get { return myValue; }
            set { }
        }

        public static class DataContainer  // static variables that can be accessed from the other tabs
        {
            public static String gchatnames;
            public static String chatgroup;
            public static int logintimes;
            public static Boolean acceptPM;
            public static Boolean notifications;
            public static Boolean PMrecieved;
            public static string PMmessage;
            public static string PMsender;
            public static Boolean multi;
        }

        public static class PMAcceptNotify  // if the variable is changed, update | method is used for real-time updating of the notification text / icon.
        {
            public static event EventHandler PMAcceptChanged;

            private static string _PMAccept;
            public static string PMAccept
            {
                get { return PMAccept; }
                set
                {
                    if (value != _PMAccept)
                    {
                        _PMAccept = value;

                        if (PMAcceptChanged != null)
                            PMAcceptChanged(null, EventArgs.Empty);
                    }
                }
            }
        }


        public static bool CallSend()  // gets whether or not a PM can be sent, and sends it to the PM window.
        { 
            finished = false;
            m_Singleton.PrivateSend();
           if (finished == true)
            {
            return true;
            }
           else
           {
               return false;
           }
        }

        public static bool SendPM(string txt)  // actually runs the send process in the PM tab
        {
            finished = false;
            m_Singleton.PMSend(txt);
            if (finished == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 



        public class NotifyData : INotifyPropertyChanged  // checks to see if the notification count has changes, and updates the notification in real time
        {
            private int _notifycount;

            public int notifycount
            {
                get { return _notifycount; }
                set { _notifycount = value; OnPropertyChanged("notifycount"); }
            }

            private int _nusers;
            public int nusers
            {
                get { return _nusers; }
                set { _nusers = value; OnPropertyChanged("nusers"); }
            }

            private int _PMctr;
            public int PMctr
            {
                get { return _PMctr; }
                set { _PMctr = value; OnPropertyChanged("PMctr"); }
            }

            //below is the boilerplate code supporting PropertyChanged events:
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        public async void Connect()
        {

            Connection = new HubConnection(ServerUrl);
            hubProxy = Connection.CreateHubProxy("NotificationHub");
            await Connection.Start();

            //Handle incoming event from server: use Invoke to write to console from SignalR's thread 
            hubProxy.On<string>("BroadcastMessage", (message) =>
                this.Dispatcher.Invoke(() =>
                   txtchat.AppendText(String.Format("{0}" + Environment.NewLine, message))
                           //m_data.notifycount++

                    )
            );
            DataContainer.notifications = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)  // send button
        {
            //MessageBox.Show(string.Format("Connected User: {0} , Notifications {1}", ConnectedUser, m_data.notifycount), "Results");
            hubProxy.Invoke("twit", this.ConnectedUser, "", TextBox1.Text);
            TextBox1.Text = String.Empty;
            TextBox1.Focus();
            m_data.notifycount = -1;

        }

        void PrivateSend()  // checks to see if message can be sent, and who it is sent to
        {
            //Connect();
            string tempname = "";

            if (DataContainer.acceptPM == true)
            {
                if (DataContainer.multi == true)
                {
                    u_data.PMctr = 0;
                    tempname = hubProxy.Invoke<string>("CreateGroup", this.ConnectedUser, DataContainer.gchatnames).Result;
                    DataContainer.chatgroup = tempname;
                    finished = true;
                    MessageBox.Show(string.Format("Single: {0}, Result: {1}, Multi: {2}, MultiVar: {3}", SingleChat_Selector.IsChecked, DataContainer.chatgroup, MultiChat_Selector.IsChecked, DataContainer.multi), "Results");
                }
                else
                {
                    u_data.PMctr = 0;
                    tempname = hubProxy.Invoke<string>("GetUniqueGroupName", this.ConnectedUser, DataContainer.gchatnames).Result;
                    DataContainer.chatgroup = tempname;
                    finished = true;
                }
            }
        
             else 
             {
                finished = false;
             }
        }
            //MessageBox.Show(string.Format("Name? {0}, Result: {1}, Tempname: {2}", PrivateChat, DataContainer.chatgroup, tempname), "Results"); 

        void PMSend(string txt)  // Sends the message after it is okayed by the PrivateSend procedure
        {
            if (DataContainer.acceptPM == true)
            {
                u_data.PMctr = 0;
                string messagetxt = txt + " {PM}";
                hubProxy.Invoke("twit", this.ConnectedUser, DataContainer.gchatnames, messagetxt);
                finished = true;
               // MessageBox.Show(string.Format("Single: {0}, Result: {1}, Multi: {2}, MultiVar: {3}", SingleChat_Selector.IsChecked, DataContainer.chatgroup, MultiChat_Selector.IsChecked, DataContainer.multi), "Results");
            }
        }


        void EnterSend(object sender, ExecutedRoutedEventArgs e)  // send button
        {
            hubProxy.Invoke("twit", this.ConnectedUser, "", TextBox1.Text);
            TextBox1.Text = String.Empty;
            TextBox1.Focus();
            m_data.notifycount = -1;
        }

        private void setuser_Click (object sender, RoutedEventArgs e)  // sets a new user and if line 273 is commented out, removes the previous username from the server
        {
            //MessageBox.Show(string.Format("Connected User: {0} , textbox: {1}", ConnectedUser, TextBox1.Text), "Results");
            if (TextBox1.Text != "")
            {
                hubProxy.Invoke("forceDisconnectCurrent"); // comment out this line to add infinite users.
                ConnectedUser = TextBox1.Text;

                TextBox1.Text = String.Empty;
                TextBox1.Focus();

                hubProxy.Invoke("getMyName", this.ConnectedUser);
    
                m_data.notifycount = 0;
                MessageBox.Show(string.Format("Username is now: {0}", ConnectedUser), "Success!");
                ServerName.Text = ("Connected to server at " + ServerURI + "  Username is " + ConnectedUser + Environment.NewLine);

            }
            else
            { 
          MessageBox.Show(string.Format("Please Enter a Username Into The TextBox "));
            }
        }

        public void PrintMessage (string sender, string message)  //Takes the incoming message and hands it off to either the private chat window, or the main window.
        {
            DataContainer.PMrecieved = false;

            if ((message.Contains('{') == true) && (message.Contains('}') == true))
            {
                DataContainer.PMrecieved = true;
                DataContainer.PMsender = sender;
                DataContainer.PMmessage = message;
                PMAcceptNotify.PMAccept = sender + ": " + message;
                //MyValue = "TEST";
                myValue = sender + ": " + message;
                txtchat.AppendText(MyValue);
                txtchat.AppendText(Environment.NewLine);
                myValue = "From: " + sender + ": " + message;
                Buttons.Instance.AppendMyText(MyValue);
                PMnotify.Visibility = Visibility.Visible;
                u_data.PMctr++;
            }
            else
            {
              txtchat.AppendText(String.Format("{0}: {1}" + Environment.NewLine, sender, message));
              DataContainer.PMrecieved = false;
            }

        }

        public async Task ConnectAsync()
        {

            hubConnection = new HubConnection(this.ServerURI);
            // hubConnection.Closed += hubConnection_Closed;
            hubProxy = hubConnection.CreateHubProxy("ChatHub");

            //Handle incoming event from server: use Invoke to write to console from SignalR's thread 
            hubProxy.On<string, string>("BroadcastMessage", (sender, message) =>
            this.Dispatcher.Invoke(() =>
            PrintMessage(sender,message)));

            hubProxy.On<string, string>("BroadcastMessage", (sender, message) =>
            this.Dispatcher.Invoke(() =>
            m_data.notifycount++));

            hubProxy.On<string, string>("BroadcastMessage", (sender, message) =>
            this.Dispatcher.Invoke(() =>
            ShowStandardBalloon(String.Format("{0}: {1}" + Environment.NewLine, sender, message))));


            hubProxy.On<string>("whoIsOn", (grps) =>
this.Dispatcher.Invoke((Action)(() => names2 = (String.Format("{0}", grps)))));

            hubProxy.On<string>("whoIsOn", (grps) =>
this.Dispatcher.Invoke((Action)(() => SplitUsers())));

            hubProxy.On<string>("whoIsOn", (grps) =>
this.Dispatcher.Invoke((Action)(() => names1 = (String.Format("{0}", grps)))));

            try
            {
                await hubConnection.Start().ContinueWith((antecedent) =>
                {
                    hubProxy.Invoke("getMyName", this.ConnectedUser);
                });
            }
            catch (HttpRequestException)
            {
                // StatusText.Text = "Unable to connect to server: Start server before connecting clients.";
                //No connection: Don't enable Send button or show chat UI 
                return;
            }


            /*
             SignInPanel.Visible = false;
             ChatPanel.Visible = true;             
             TextBoxMessage.Focus();
              */
            ServerName.Text = ("Connected to server at " + ServerURI + "  Default Username is " + ConnectedUser + Environment.NewLine);

        }

        public void SplitUsers()  // takes the string of connected users and splits it into each username.
        {
            u_data.nusers = 0;
            char[] delimiterChars = { ' ', ',', ';', ':', '\t' };
            string[] words = names2.Split(delimiterChars);
            Userslist.Text = "Online Users: ";

            foreach (string s in words)
            {
                u_data.nusers++;
                Userslist.Text += (String.Format(Environment.NewLine + "{0}", s));
            }

        }


        private void MainWindow_Load(object sender, EventArgs e)
        {
            Connect();

        }


                    private void TabablzControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)  // if the active tab is changed.
        {

        }

                    private void UserBox_DropDownOpened(object sender, EventArgs e)  // open up the select box to choose a user to send a PM to
                    {
                        List<string> blank = "".Split(';').ToList<string>();

                        UserBox.ItemsSource = blank;

                        //UserBox.Items.Clear();
                        List<string> names = names1.Split(';').ToList<string>();

                        UserBox.ItemsSource = names;

                        //UserList.ItemsSource = names;

                    }


                      private void ShowStandardBalloon(string text)  // this shows the notification from the taskbar
                    {
                        if ((DataContainer.notifications == true) && (m_data.notifycount > 0))
                            {
                                FancyBalloon balloon = new FancyBalloon();
                                balloon.BalloonText = "You Have A New Message";
                                balloon.BalloonMessage = text + " Notifications: " + m_data.notifycount;
                                tb.ShowCustomBalloon(balloon, PopupAnimation.Slide, 4000);   //show and close after 2.5 seconds (5000)
                            }
                        
                    }

                      private void RadioButton_Multi(object sender, RoutedEventArgs e)  // sets the PM type to Multi chat
                      {
                          UserBox.Visibility = Visibility.Collapsed;
                          UserList.Visibility = Visibility.Visible;
                          privateusers.Visibility = Visibility.Collapsed;
                          
                          List<string> blank = "".Split(';').ToList<string>();
                          UserList.ItemsSource = blank;
                          List<string> names = names1.Split(';').ToList<string>();
                          UserList.ItemsSource = names;
                          DataContainer.acceptPM = false;
                      }

                      private void RadioButton_Single(object sender, RoutedEventArgs e) //sets the PM type to single chat
                      {
                          UserBox.Visibility = Visibility.Visible;
                          UserList.Visibility = Visibility.Collapsed;
                          privateusers.Visibility = Visibility.Collapsed;
                          DataContainer.acceptPM = false;
                      }

                      private void AcceptChat_click(object sender, RoutedEventArgs e) // accept settings + start a new chat
                      {
                          UserBox.Visibility = Visibility.Collapsed;
                          UserList.Visibility = Visibility.Collapsed;
                          privateusers.Visibility = Visibility.Visible;
                          if (SingleChat_Selector.IsChecked == true)
                          {
                              privateusers.Text = (String.Format("Connected Users: "+  "{0}" + Environment.NewLine, UserBox.SelectedItem));
                              
                              chatnames = (String.Format("{0}", UserBox.SelectedItem));
                              DataContainer.gchatnames = chatnames;
                              DataContainer.logintimes = 0;
                              DataContainer.acceptPM = true;
                              DataContainer.multi = false;
                          }
                          else
                          {
                              var listarray = new System.Collections.ArrayList(UserList.SelectedItems);
                              var selectedstring = String.Join(", ", UserList.SelectedItems.Cast<string>());

                             // var selectedstring = String.Join(",", listarray);
                              privateusers.Text = (String.Format("Connected Users: " + "{0}" + Environment.NewLine, selectedstring));

                              chatnames = (String.Format("{0}", selectedstring));
                              DataContainer.gchatnames = chatnames;
                              DataContainer.acceptPM = true;
                              DataContainer.multi = true;
                          }
                           DataContainer.acceptPM = true;
                      }

                      private void CancelChat_click(object sender, RoutedEventArgs e) // cancel current chatroom, and log out of it
                      {
                          UserBox.Visibility = Visibility.Visible;
                          UserList.Visibility = Visibility.Collapsed;
                          SingleChat_Selector.IsChecked = true;
                          privateusers.Visibility = Visibility.Collapsed;
                          DataContainer.acceptPM = false;

                      }

                      private void ClearNotifications(object sender, RoutedEventArgs e) // set unread notification count to 0
                      {
                          m_data.notifycount = 0;
                      }


                      private void SilentOn_Click(object sender, RoutedEventArgs e)  // toggle notifications
                      {
                          if (DataContainer.notifications == true)
                          {
                              DataContainer.notifications = false;
                              SilentOff.Visibility = Visibility.Collapsed;
                              SilentToggle.Visibility = Visibility.Visible;
                          }
                          else
                          {
                              DataContainer.notifications = true;
                              SilentOff.Visibility = Visibility.Visible;
                              SilentToggle.Visibility = Visibility.Collapsed;
                          }
                      }

                      private void UserBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // if a diffrent person is chosen to be sent a PM
                      {
                          List<string> blank = "".Split(';').ToList<string>();
                          //blank = UserBox.SelectedValue;
                          //chatnames = UserBox.SelectedItem;
                          chatnames = UserBox.Text;
                          //PrivateChat.Text = chatnames;
                      //f1.PrivateChat.Text += chatnames;
                      }


    }
}
