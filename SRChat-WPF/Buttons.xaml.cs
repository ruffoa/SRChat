using Microsoft.AspNet.SignalR.Client;
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
using System.Net.Http;
using System.ComponentModel;

namespace SRChat_WPF
{
    /// <summary>
    /// Interaction logic for Buttons.xaml
    /// </summary>
    public partial class Buttons : UserControl
    {
        static Buttons instance;
        public static Buttons Instance { get { return instance; } }

        //MainWindow f1 = new MainWindow();
        public string chatnames2 { get; set; }
        public int logintimes { get; set; }

        public Buttons()
        {
            InitializeComponent();
            logintimes = 0;
            instance = this;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //PrivateChat.Text = f1.privateusers.Text;
            //PrivateChat.Text +=  (String.Format("Connected Users: " + "{0}" + Environment.NewLine, f1.chatnames));
            if (SRChat_WPF.MainWindow.DataContainer.logintimes == 0)
            {
                if (PrivateChat.Text != "") // if the chat box is not empty
                {
                    PrivateChat.Text += "___________________________________________________________________________________________" + Environment.NewLine ;
                    PrivateChat.Text += (String.Format("Connected to: " + "{0}", SRChat_WPF.MainWindow.DataContainer.gchatnames));
                    SRChat_WPF.MainWindow.CallSend();
                    if (SRChat_WPF.MainWindow.DataContainer.chatgroup == "0")
                    {
                        PrivateChat.Text += (String.Format(" In Chat Room: " + "{0}" + " (local)"+ Environment.NewLine, SRChat_WPF.MainWindow.DataContainer.chatgroup));
                    }
                    else
                    {
                        PrivateChat.Text += (String.Format(" In Chat Room: " + "{0}" + Environment.NewLine, SRChat_WPF.MainWindow.DataContainer.chatgroup));
                    }
                        SRChat_WPF.MainWindow.DataContainer.logintimes++;
                }
                else if (PrivateChat.Text == "")  // if the chat box is empty
                {
                    PrivateChat.Text += (String.Format("Connected to: " + "{0}", SRChat_WPF.MainWindow.DataContainer.gchatnames));
                    SRChat_WPF.MainWindow.CallSend();
                    if (SRChat_WPF.MainWindow.DataContainer.chatgroup == "0")
                    {
                        PrivateChat.Text += (String.Format(" In Chat Room: " + "{0}" + " (local)" + Environment.NewLine, SRChat_WPF.MainWindow.DataContainer.chatgroup));
                    }
                    else
                    {
                        PrivateChat.Text += (String.Format(" In Chat Room: " + "{0}" + Environment.NewLine, SRChat_WPF.MainWindow.DataContainer.chatgroup));
                    }
                    SRChat_WPF.MainWindow.DataContainer.logintimes++;
                }
            }
            if (PrivateMessage.Text != "")  // if the message box is not empty
            {
                //SRChat_WPF.MainWindow.SendPM(PrivateMessage.Text);
                if (SRChat_WPF.MainWindow.SendPM(PrivateMessage.Text) == true)
                {
                    PrivateChat.Text += "To: " + SRChat_WPF.MainWindow.DataContainer.gchatnames + ": " + PrivateMessage.Text + Environment.NewLine;
                }
                else
                {
                    PrivateChat.Text += "Fail to send message. Check your Internet Connection. (404)" + Environment.NewLine;
                }
                PrivateMessage.Text = "";
            }
        }

        //public event SRChat_WPF.MainWindow.DataContainer.PMmessageChanged;
        //{
        //    PrivateChat.Text += "WORKING? " + SRChat_WPF.MainWindow.DataContainer.PMmessage + Environment.NewLine;
        //}

        public void AppendMyText(string text) // adds on the text to the textbox as an attempt to allow adding of text from the main program.
        {
            PrivateChat.AppendText(text);
            PrivateChat.AppendText(Environment.NewLine);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }


    }
}
