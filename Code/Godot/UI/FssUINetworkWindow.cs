using Godot;
using System;

public partial class FssUINetworkWindow : Window
{
    Label    UdpIpAddrLabel;
    Label    UdpIpPortLabel;
    LineEdit UdpIpAddrEdit;
    LineEdit UdpIpPortEdit;
    Button   UdpIpConnectButton;

    Label    TcpIpServerAddrLabel;
    Label    TcpIpServerPortLabel;
    LineEdit TcpIpServerAddrEdit;
    LineEdit TcpIpSerfverPortEdit;
    Button   TcpIpServerConnectButton;

    Label    TcpIpClientAddrLabel;
    Label    TcpIpClientPortLabel;
    LineEdit TcpIpClientAddrEdit;
    LineEdit TcpIpClientPortEdit;
    Button   TcpIpClientConnectButton;

    CheckBox MaintainConnectionCheckBox;

    TextEdit NetworkStatusTextEdit;

    Button OkButton;
    Button CancelButton;

    float TimerUIUpdate = 0.0f;

    // --------------------------------------------------------------------------------------------
    // MARK: Node Functions
    // --------------------------------------------------------------------------------------------

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        UdpIpAddrLabel       = (Label)FindChild("UdpIpAddrLabel");
        UdpIpPortLabel       = (Label)FindChild("UdpIpPortLabel");
        UdpIpAddrEdit        = (LineEdit)FindChild("UdpIpAddrEdit");
        UdpIpPortEdit        = (LineEdit)FindChild("UdpIpPortEdit");
        UdpIpConnectButton   = (Button)FindChild("UdpIpConnectButton");

        TcpIpServerAddrLabel = (Label)FindChild("TcpIpServerAddrLabel");
        TcpIpServerPortLabel = (Label)FindChild("TcpIpServerPortLabel");
        TcpIpServerAddrEdit  = (LineEdit)FindChild("TcpIpServerAddrEdit");
        TcpIpSerfverPortEdit = (LineEdit)FindChild("TcpIpSerfverPortEdit");
        TcpIpServerConnectButton = (Button)FindChild("TcpIpServerConnectButton");

        TcpIpClientAddrLabel = (Label)FindChild("TcpIpClientAddrLabel");
        TcpIpClientPortLabel = (Label)FindChild("TcpIpClientPortLabel");
        TcpIpClientAddrEdit  = (LineEdit)FindChild("TcpIpClientAddrEdit");
        TcpIpClientPortEdit  = (LineEdit)FindChild("TcpIpClientPortEdit");
        TcpIpClientConnectButton = (Button)FindChild("TcpIpClientConnectButton");

        MaintainConnectionCheckBox = (CheckBox)FindChild("MaintainConnectionCheckBox");

        NetworkStatusTextEdit = (TextEdit)FindChild("NetworkStatusTextEdit");

        OkButton     = (Button)FindChild("OkButton");
        CancelButton = (Button)FindChild("CancelButton");

        OkButton.Connect("pressed", new Callable(this, "OnOkButtonPressed"));
        CancelButton.Connect("pressed", new Callable(this, "OnCancelButtonPressed"));

        // Connect the close_requested signal to the OnCloseRequested function
        Connect("close_requested", new Callable(this, "OnCancelButtonPressed"));

        var config = FssCentralConfig.Instance;
        string configUdpIpAddr = config.GetParam<string>("UdpIpAddr", "127.0.0.1");
        int    configUdpIpPort = config.GetParam<int>("UdpIpPort", 10001);

        UdpIpConnectButton.Connect("pressed", new Callable(this, "OnUdpIpConnectButtonPressed"));
        TcpIpServerConnectButton.Connect("pressed", new Callable(this, "OnTcpIpServerConnectButtonPressed"));
        TcpIpClientConnectButton.Connect("pressed", new Callable(this, "OnTcpIpClientConnectButtonPressed"));

        UpdateUIText();
        PopulateDialogControls();
        MaintainConnections();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (TimerUIUpdate < FssCentralTime.RuntimeSecs)
        {
            TimerUIUpdate = FssCentralTime.RuntimeSecs + 2f; // Update the timer to the next whole second

            NetworkStatusTextEdit.Text = FssEventDriver.NetworkReport();

            UpdateUIText();
        }
    }

    // --------------------------------------------------------------------------------------------
    // MARK: MaintainConnections
    // --------------------------------------------------------------------------------------------

    // Function called on startup to perform the maintain connections process

    public void MaintainConnections()
    {
        var config = FssCentralConfig.Instance;

        bool maintainConnections = config.GetParam<bool>("MaintainConnections", false);
        MaintainConnectionCheckBox.SetPressedNoSignal( maintainConnections );

        if (maintainConnections)
        {
            bool udpConnectionOn       = config.GetParam<bool>("MaintainConnections_UDP", false);
            bool tcpServerConnectionOn = config.GetParam<bool>("MaintainConnections_TCPServer", false);
            bool tcpClientConnectionOn = config.GetParam<bool>("MaintainConnections_TCPClient", false);

            if (udpConnectionOn)
            {
                UdpIpConnectButton.SetPressedNoSignal( true );
                OnUdpIpConnectButtonPressed();
            }

            if (tcpServerConnectionOn)
            {
                TcpIpServerConnectButton.SetPressedNoSignal( true );
                OnTcpIpServerConnectButtonPressed();
            }

            if (tcpClientConnectionOn)
            {
                TcpIpClientConnectButton.SetPressedNoSignal( true );
                OnTcpIpClientConnectButtonPressed();
            }
        }
    }

    // --------------------------------------------------------------------------------------------
    // MARK: Data Read and Write
    // --------------------------------------------------------------------------------------------

    private void PopulateDialogControls()
    {
        var config = FssCentralConfig.Instance;
        UdpIpAddrEdit.Text        = config.GetParam<string>("UdpIpAddr", "127.0.0.1");
        UdpIpPortEdit.Text        = config.GetParam<int>("UdpIpPort", 10001).ToString();

        TcpIpServerAddrEdit.Text  = config.GetParam<string>("TcpIpServerAddr", "127.0.0.1");
        TcpIpSerfverPortEdit.Text = config.GetParam<int>("TcpIpServerPort", 10002).ToString();

        TcpIpClientAddrEdit.Text  = config.GetParam<string>("TcpIpClientAddr", "127.0.0.1");
        TcpIpClientPortEdit.Text  = config.GetParam<int>("TcpIpClientPort", 10003).ToString();
    }

    private void SaveControlValues()
    {
        // int udpIpPort = 0;
        // int tcpIpServerPort = 0;
        // int tcpIpClientPort = 0;

        int.TryParse(UdpIpPortEdit.Text, out int udpIpPort);
        int.TryParse(TcpIpSerfverPortEdit.Text, out int tcpIpServerPort);
        int.TryParse(TcpIpClientPortEdit.Text, out int tcpIpClientPort);


        var config = FssCentralConfig.Instance;
        config.SetParam("UdpIpAddr", UdpIpAddrEdit.Text);
        config.SetParam("UdpIpPort", udpIpPort);

        config.SetParam("TcpIpServerAddr", TcpIpServerAddrEdit.Text);
        config.SetParam("TcpIpServerPort", tcpIpServerPort);

        config.SetParam("TcpIpClientAddr", TcpIpClientAddrEdit.Text);
        config.SetParam("TcpIpClientPort", tcpIpClientPort);

        config.SetParam("MaintainConnections", MaintainConnectionCheckBox.IsPressed());

        config.SetParam("MaintainConnections_UDP",      UdpIpConnectButton.IsPressed());
        config.SetParam("MaintainConnections_TCPServer", TcpIpServerConnectButton.IsPressed());
        config.SetParam("MaintainConnections_TCPClient", TcpIpClientConnectButton.IsPressed());
    }

    // --------------------------------------------------------------------------------------------
    // MARK: Localisation
    // --------------------------------------------------------------------------------------------

    private void UpdateUIText()
    {
        Title                         = FssLanguageStrings.Instance.GetParam("Network");

        UdpIpAddrLabel.Text           = FssLanguageStrings.Instance.GetParam("UdpIpAddr");
        UdpIpPortLabel.Text           = FssLanguageStrings.Instance.GetParam("Port");
        UdpIpConnectButton.Text       = FssLanguageStrings.Instance.GetParam("Connect");

        TcpIpServerAddrLabel.Text     = FssLanguageStrings.Instance.GetParam("TcpIpServerAddr");
        TcpIpServerPortLabel.Text     = FssLanguageStrings.Instance.GetParam("Port");
        TcpIpServerConnectButton.Text = FssLanguageStrings.Instance.GetParam("Connect");

        TcpIpClientAddrLabel.Text     = FssLanguageStrings.Instance.GetParam("TcpIpClientAddr");
        TcpIpClientPortLabel.Text     = FssLanguageStrings.Instance.GetParam("Port");
        TcpIpClientConnectButton.Text = FssLanguageStrings.Instance.GetParam("Connect");

        MaintainConnectionCheckBox.Text = FssLanguageStrings.Instance.GetParam("MaintainConnection");
        //NetworkStatusTextEdit.Text      = FssLanguageStrings.Instance.GetParam("NetworkStatus");

        OkButton.Text     = FssLanguageStrings.Instance.GetParam("Ok");
        CancelButton.Text = FssLanguageStrings.Instance.GetParam("Cancel");
    }

    // --------------------------------------------------------------------------------------------
    // MARK: UI Interactions
    // --------------------------------------------------------------------------------------------

    public void OnOkButtonPressed()
    {
        FssCentralLog.AddEntry("FssUINetworkWindow.OnOkButtonPressed");
        Visible = false;

        // Save the control values to the config file
        SaveControlValues();
        FssCentralConfig.Instance.WriteToFile();
    }
    public void OnCancelButtonPressed()
    {
        FssCentralLog.AddEntry("FssUINetworkWindow.OnCancelButtonPressed");
        Visible = false;

        // Update the controls from values in the config file
        PopulateDialogControls();
    }

    // --------------------------------------------------------------------------------------------
    // MARK: UI Interactions - Connections
    // --------------------------------------------------------------------------------------------

    public void OnUdpIpConnectButtonPressed()
    {
        // first check the status is now pressed
        if ( UdpIpConnectButton.IsPressed() )
        {
            FssCentralLog.AddEntry("FssUINetworkWindow.OnUdpIpConnectButtonPressed");

            string ipAddr = UdpIpAddrEdit.Text;
            int   port   = int.Parse(UdpIpPortEdit.Text);

            FssEventDriver.NetworkConnect("UdpReceiver", "UdpReceiver", ipAddr, port);
        }
        else // Else disconnect
        {
            FssCentralLog.AddEntry("FssUINetworkWindow.OnUdpIpConnectButtonPressed - Disconnect");
            FssEventDriver.NetworkDisconnect("UdpReceiver");
        }
        FssCentralLog.AddEntry("FssUINetworkWindow.OnUdpIpConnectButtonPressed");
    }

    public void OnTcpIpServerConnectButtonPressed()
    {
        // first check the status is now pressed
        if (TcpIpServerConnectButton.IsPressed())
        {
            FssCentralLog.AddEntry("FssUINetworkWindow.OnTcpIpServerConnectButtonPressed");

            string ipAddr = TcpIpServerAddrEdit.Text;
            int   port   = int.Parse(TcpIpSerfverPortEdit.Text);

            FssEventDriver.NetworkConnect("TcpServer", "TcpServer", ipAddr, port);
        }
        else // Else disconnect
        {
            FssCentralLog.AddEntry("FssUINetworkWindow.OnTcpIpServerConnectButtonPressed - Disconnect");
            FssEventDriver.NetworkDisconnect("TcpServer");
        }
    }

    public void OnTcpIpClientConnectButtonPressed()
    {
        // first check the status is now pressed
        if (TcpIpClientConnectButton.IsPressed())
        {
            FssCentralLog.AddEntry("FssUINetworkWindow.OnTcpIpClientConnectButtonPressed");

            string ipAddr = TcpIpClientAddrEdit.Text;
            int   port   = int.Parse(TcpIpClientPortEdit.Text);

            FssEventDriver.NetworkConnect("TcpClient", "TcpClient", ipAddr, port);
        }
        else // Else disconnect
        {
            FssCentralLog.AddEntry("FssUINetworkWindow.OnTcpIpClientConnectButtonPressed - Disconnect");
            FssEventDriver.NetworkDisconnect("TcpClient");
        }
    }

}
