using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Data;


namespace CUESYSv._01
{
    public partial class Form1 : Form
    {
        ///// NOTES START //////////////////////////////////////////////////////////
        // Should include log items stored on database
        // Bookings only in single hour "slots", would be better to custom set
        // Cannot search for booking (by room, date or customer)
        // Only view and edit upcoming x days
        // User+Pass check, insecure - later versions should use a database lookup
        // formatting odd on maximize/resize/different screen resolutions
        // menu is shown when software ran, this allows modification of customer entries before login - not secure
        // devlog out of sync with actions
        // autoscroll devlogs
        // need to hide menu bar on start (good for debug though)
        ///// NOTES END ////////////////////////////////////////////////////////////


        ///// VARIABLES START //////////////////////////////////////////////////////
        dbConn mysqlConn = new dbConn();
        private string varFloor;
        private string varRoom;
        ///// VARIABLES END ////////////////////////////////////////////////////////

        ///// METHODS START ////////////////////////////////////////////////////////
        public void devLogs(string logItem)
        {//Write development log to DevLog
            using (StreamWriter devlog = new StreamWriter("DevLog.txt", append: true))
            { devlog.WriteLine(DateTime.Now + " --- " + logItem); }//Concat current time and logItem and write to DevLog file
        }
        public bool dbConfig()
        {
            try
            {
                mysqlConn.varConfigServer = "ac8453.cucstudents.org";
                mysqlConn.varConfigDatabase = "BtBooking";
                mysqlConn.varConfigUser = "BtBooking";
                mysqlConn.varConfigPass = "Password123!";
                return true;
            }
            catch { return false; }
        }

        public void resetControls(string newFocus)
        {//Hide all controls and only show those needed
            devLogs("resetControls triggered");
            foreach (Control control in this.Controls) { control.Visible = false; }//Hide all controls
            lbCueSys.Visible = true;//Show logo
            panClock.Visible = true;//Show clock panel
            mainMenu.Visible = true;//Show menu
            myDarkBg.Visible = true;//show bg
            foreach (var clockLbl in panClock.Controls.OfType<Label>()) { clockLbl.Visible = true; };//Show clock in panel
            switch (newFocus)//Use control statement to selectively show controls based on newFocus argument
            {
                case "Program started":
                    lbUserName.Visible = lbUserPass.Visible = tbUserName.Visible = tbUserPass.Visible = btLogin.Visible = true;//make login controls visible
                    devLogs("Login controls visible");
                    break;
                case "landing":
                    
                    dgRoomBookingsSummary.Visible = true;
                    // updateButton.Visible = true;
                    deleteButton.Visible = true;
                    dbReturn("SELECT * FROM `tblBookings` WHERE `bookingDateTime` >= CURDATE()");
                    break;
                case "Book Room":
                    panFloorLayout.Visible = true;
                    cbBuilding.Visible = true;
                    cbFloor.Visible = true;
                    foreach (var x in panFloorLayout.Controls.OfType<Button>())
                    {//Make each button transparent
                        x.Parent = panFloorLayout;
                        x.Visible = true;
                        x.BackColor = Color.Transparent;
                        x.FlatAppearance.MouseDownBackColor = Color.Transparent;
                        x.FlatAppearance.MouseOverBackColor = Color.Transparent;
                        x.FlatStyle = FlatStyle.Flat;
                        x.ForeColor = BackColor;
                        x.UseVisualStyleBackColor = true;
                        x.FlatAppearance.BorderSize = 0;
                    };
                    break;
                case "create customer":
                    lbCustAdd1.Visible = true;
                    lbCustAdd2.Visible = true;
                    lbCustContact.Visible = true;
                    lbCustEmail.Visible = true;
                    lbCustPostcode.Visible = true;
                    lbCustTel.Visible = true;
                    lbCustNationality.Visible = true;
                    lbCustTitle.Visible = true;
                    lbCustTownCity.Visible = true;
                    tbCustAdd1.Visible = true;
                    tbCustAdd2.Visible = true;
                    tbCustContact.Visible = true;
                    tbCustEmail.Visible = true;
                    tbCustPostcode.Visible = true;
                    tbCustTel.Visible = true;
                    tbCustNationality.Visible = true;
                    tbCustTownCity.Visible = true;
                    btCustSave.Visible = true;
                    lbCustTitle.Text = "Create Customer";
                    break;
                case "edit customer":
                    lbCustAdd1.Visible = true;
                    lbCustAdd2.Visible = true;
                    lbCustContact.Visible = true;
                    lbCustEmail.Visible = true;
                    lbCustPostcode.Visible = true;
                    lbCustTel.Visible = true;
                    lbCustNationality.Visible = true;
                    lbCustTitle.Visible = true;
                    lbCustTownCity.Visible = true;
                    tbCustAdd1.Visible = true;
                    tbCustAdd2.Visible = true;
                    tbCustContact.Visible = true;
                    tbCustEmail.Visible = true;
                    tbCustPostcode.Visible = true;
                    tbCustTel.Visible = true;
                    tbCustNationality.Visible = true;
                    tbCustTownCity.Visible = true;
                    btCustEdit.Visible = true;
                    lbCustTitle.Text = "Update Customer";
                    break;
                case "view customers":
                    //show all customers
                    dgRoomBookingsSummary.Visible = true;
                    updateButton.Visible = true;
                    deleteButton.Visible = true;
                    dbReturn("SELECT * FROM `tblCustomer`");
                    break;
                case "book flight":
                    //show book flight forms and lables
                    myDarkBg.Visible = true;
                    lbBookFlight.Visible = true;
                    lbFlightCustContact.Visible = true;
                    lbFlightAirline.Visible = true;
                    lbFlightOrigin.Visible = true;
                    lbFlightDestination.Visible = true;
                    lbFlightNumber.Visible = true;
                    lbFlightSeat.Visible = true;
                    lbFlightAdult.Visible = true;
                    lbFlightChildren.Visible = true;
                    lbFlightInfant.Visible = true;
                    lbFlightBookingPaid.Visible = true;
                    lbFlightBookDate.Visible = true;

                    tbFlightCustContact.Visible = true;
                    tbFlightAirline.Visible = true;
                    tbFlightOrigin.Visible = true;
                    tbFlightDestination.Visible = true;
                    tbFlightNumber.Visible = true;
                    tbFlightSeat.Visible = true;
                    tbFlightAdult.Visible = true;
                    tbFlightChildren.Visible = true;
                    tbFlightInfant.Visible = true;
                    cbFlightPaid.Visible = true;
                    dtFlightBook.Visible = true;
                    btnFlightSave.Visible = true;

                    break;
                case "view flights":
                    //show all flights
                    dgRoomBookingsSummary.Visible = true;
                    updateButton.Visible = true;
                    deleteButton.Visible = true;
                    dbReturn("SELECT * FROM `tblflights`");
                    break;
                case "search flight query":
                    if (tbSearchFlight.TextLength > 0)
                    {
                        string searchQuery = tbSearchFlight.Text;
                        string queryFullText = "SELECT * FROM `tblflights` WHERE custContact LIKE " + "\"" + searchQuery + "\"" + " OR airLine LIKE " + "\"" + searchQuery + "\"" + " OR flightNumber LIKE " + "\"" + searchQuery + "\"" + " OR seatNumber LIKE " + "\"" + searchQuery + "\"" + " OR flightOrigin LIKE " + "\"" + searchQuery + "\"" + " OR flightDestination LIKE " + "\"" + searchQuery + "\"";
                        devLogs(queryFullText);
                        dgRoomBookingsSummary.Visible = true;
                        dbReturn(queryFullText);
                    }
                    break;
                case "search flight":
                    myDarkBg.Visible = true;
                    tbSearchFlight.Visible = true;
                    btSearchFlight.Visible = true;
                    break;
                case "edit flight":
                    myDarkBg.Visible = true;
                    lbEditFlight.Visible = true;
                    lbFlightCustContact.Visible = true;
                    lbFlightAirline.Visible = true;
                    lbFlightOrigin.Visible = true;
                    lbFlightDestination.Visible = true;
                    lbFlightNumber.Visible = true;
                    lbFlightSeat.Visible = true;
                    lbFlightAdult.Visible = true;
                    lbFlightChildren.Visible = true;
                    lbFlightInfant.Visible = true;
                    lbFlightBookingPaid.Visible = true;
                    lbFlightBookDate.Visible = true;

                    tbFlightCustContact.Visible = true;
                    tbFlightAirline.Visible = true;
                    tbFlightOrigin.Visible = true;
                    tbFlightDestination.Visible = true;
                    tbFlightNumber.Visible = true;
                    tbFlightSeat.Visible = true;
                    tbFlightAdult.Visible = true;
                    tbFlightChildren.Visible = true;
                    tbFlightInfant.Visible = true;
                    cbFlightPaid.Visible = true;
                    dtFlightBook.Visible = true;
                    btnEditFlightSave.Visible = true;
                    break;
                case "Exit":
                    Application.Exit();
                    break;
                default:
                    devLogs("resetControls default case triggered, no controls visible");
                    break;
            }
            devLogs("Focus changed to " + newFocus);
        }
        public void dbReturn(string returnWhat)
        {
            devLogs(returnWhat + " sql run");
            if (mysqlConn.connOpen() == true)
            {
                dgRoomBookingsSummary.DataSource = mysqlConn.qry(returnWhat).Tables[0];
            }
        }
        public void createBooking(string room)
        {
            resetControls("");
            switch (cbFloor.Text)
            {
                case "Ground":
                    varFloor = "G";
                    break;
                case "First":
                    varFloor = "1";
                    break;
                case "Second":
                    varFloor = "2";
                    break;
                case "Third":
                    varFloor = "3";
                    break;
                case "Fourth":
                    varFloor = "4";
                    break;
                default:
                    break;
            }
            lbBookingInfo.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            tbCustomer.Visible = true;
            mcDate.Visible = true;
            tbTime.Visible = true;
            tbCost.Visible = true;
            cbPaid.Visible = true;
            btBook.Visible = true;
            mcDate.MaxSelectionCount = 1;
            lbBookingInfo.Text = cbBuilding.Text + " - " + varFloor + room;
            varRoom = room;
        }
        ///// METHODS END //////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            File.WriteAllText("DevLog.txt", String.Empty);//Clear contents of DevLog
            lbCueSys.Font = new Font("Comic Sans MS", 40, FontStyle.Bold);
            this.ActiveControl = tbUserName;
            dbConfig();
            mysqlConn.connect();
            resetControls("Program started");
            devLogs("Program started");
        }

        ///// EVENTS START /////////////////////////////////////////////////////////
        private void timeClock_Tick(object sender, EventArgs e)
        {//Timer to control clock
            lbClockTime.Text = DateTime.Now.ToString("HH:mm");
            lbClockSeconds.Text = DateTime.Now.ToString("ss");
            lbClockDate.Text = DateTime.Now.ToString("ddd") + "  " + DateTime.Now.ToString("dd/MM/yyyy");
        }


        private void btLogin_Click(object sender, EventArgs e)
        {
            devLogs("Login button clicked");
            //User+Pass check, not secure and only allows one login
            if (tbUserName.Text == "admin" && tbUserPass.Text == "admin")
            { resetControls("view customers"); devLogs("Login success for user " + tbUserName.Text); }//Login success
            else
            { MessageBox.Show("Sorry, wrong password/user combo!"); devLogs("Login failure for user " + tbUserName.Text); }//Login failure
            tbUserName.Text = ""; tbUserPass.Text = ""; //Clear logon credentials
        }
        private void tbUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {//Change focus to password box on enter key
                this.ActiveControl = tbUserPass;
                devLogs("enter key detected in tbUserName");
            }
        }
        private void tbUserPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {//Trigger login button on enter key
                btLogin_Click(this, new EventArgs());
                devLogs("enter key detected in tbUserPass");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {//Generic keyboard shortcuts
            if (keyData == (Keys.Alt | Keys.L))
            {
                devLogs("alt-l shortcut intercepted");
                resetControls("Program started");
                return true;
            }
            if (keyData == (Keys.Alt | Keys.X))
            {
                devLogs("alt-x shortcut intercepted");
                resetControls("Exit");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void viewDevLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form devForm = new Form();
            devForm.Text = "DevLogs";
            RichTextBox rtbDevLogs = new RichTextBox();
            Timer timerRefreshDevLogs = new Timer();
            timerRefreshDevLogs.Interval = 2500;
            timerRefreshDevLogs.Tick += new EventHandler(devRefreshTimer_Tick);
            timerRefreshDevLogs.Start();
            rtbDevLogs.Location = new Point(0, 0);
            rtbDevLogs.Size = new Size(300, 380);
            rtbDevLogs.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            devForm.Size = new Size(300, 400);
            devForm.Controls.Add(rtbDevLogs);
            devLogs("devlogs viewed");
            void devRefreshTimer_Tick(object timer, EventArgs args)
            {
                rtbDevLogs.Text = "";
                string line;
                try
                {
                    StreamReader sr = new StreamReader("DevLog.txt");
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        rtbDevLogs.Text += line + "\r\n";
                        line = sr.ReadLine();
                    }
                    sr.Close();
                }
                catch (Exception ex) { devLogs("error reading devlogs"); }
            }
            devForm.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetControls("Program started"); devLogs("user logged out");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetControls("Exit"); devLogs("application exit request");
        }

        private void bookRoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetControls("Book Room"); devLogs("book room request");
        }

        private void btRoomA_Click(object sender, EventArgs e)
        {
            createBooking("1");
            devLogs("room a clicked");
        }

        private void btRoomB_Click(object sender, EventArgs e)
        {
            createBooking("2");
            devLogs("room b clicked");
        }

        private void btRoomC_Click(object sender, EventArgs e)
        {
            createBooking("3");
            devLogs("room c clicked");
        }

        private void btRoomD_Click(object sender, EventArgs e)
        {
            createBooking("4");
            devLogs("room d clicked");
        }

        private void btRoomE_Click(object sender, EventArgs e)
        {
            createBooking("5");
            devLogs("room e clicked");
        }

        private void btRoomF_Click(object sender, EventArgs e)
        {
            createBooking("6");
            devLogs("room f clicked");
        }

        private void btRoomG_Click(object sender, EventArgs e)
        {
            createBooking("7");
            devLogs("room g clicked");
        }

        private void btRoomH_Click(object sender, EventArgs e)
        {
            createBooking("8");
            devLogs("room h clicked");
        }

        private void createCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetControls("create customer"); devLogs("create customer request");
        }

        private void viewBookingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetControls("landing"); devLogs("show bookings");
        }

        private void viewCustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetControls("view customers"); devLogs("show customers");
        }

        private void btCustSave_Click(object sender, EventArgs e)
        {
            devLogs("insert new customer");
            if (mysqlConn.connOpen() == true)
            {
                mysqlConn.insertCustomer(tbCustContact.Text, tbCustEmail.Text, tbCustTel.Text, tbCustNationality.Text, tbCustAdd1.Text, tbCustAdd2.Text, tbCustTownCity.Text, tbCustPostcode.Text);
            }
            tbCustContact.Text = tbCustEmail.Text = tbCustTel.Text = tbCustNationality.Text = tbCustAdd1.Text = tbCustAdd2.Text = tbCustTownCity.Text = tbCustPostcode.Text = "";
            resetControls("view customers");
        }

        private void dgRoomBookingsSummary_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            /*
            devLogs("booking doubleclicked");
            devLogs(dgRoomBookingsSummary.Columns[0].Name);
            if (dgRoomBookingsSummary.Columns[0].Name == "bookingID") {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this booking?", "Delete booking", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    mysqlConn.deleteBooking(Convert.ToString(dgRoomBookingsSummary.CurrentRow.Cells[0].Value));
                }
                resetControls("landing");
            }
            if (dgRoomBookingsSummary.Columns[0].Name == "custID")
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this customer?", "Delete customer", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    mysqlConn.deleteCustomer(Convert.ToString(dgRoomBookingsSummary.CurrentRow.Cells[0].Value));
                }
                resetControls("view customers");
            }
            if (dgRoomBookingsSummary.Columns[0].Name == "flightID")
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this flight?", "Delete flight", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    mysqlConn.deleteFlight(Convert.ToString(dgRoomBookingsSummary.CurrentRow.Cells[0].Value));
                }
                resetControls("view flights");
            }*/
        }

        private void btBook_Click(object sender, EventArgs e)
        {
            string varDateTime = mcDate.SelectionRange.Start.ToString("yyyy-MM-dd") + " " + tbTime.Text + ":00";
            string varPaid; ;
            if (cbPaid.Checked == true) { varPaid = "Y"; }
            else { varPaid = "N"; }
            if (mysqlConn.connOpen() == true)
            {
                mysqlConn.insertBooking(tbCustomer.Text, cbBuilding.Text, varFloor, varRoom, varDateTime, tbCost.Text, varPaid);
            }
            resetControls("landing");
        }


        private void bookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Add_flight_booking addFlightBooking = new Add_flight_booking();
            //addFlightBooking.ShowDialog();
            resetControls("book flight"); devLogs("book flight request");

        }

        private void btnFlightSave_Click(object sender, EventArgs e)
        {
            devLogs("insert new flight");
            string varDateTime = dtFlightBook.SelectionRange.Start.ToString("yyyy-MM-dd") + " " + tbTime.Text + ":00"; ;
            string varPaid;
            if (cbFlightPaid.Checked == true) { varPaid = "Y"; }
            else { varPaid = "N"; }
            if (mysqlConn.connOpen() == true)
            {
                mysqlConn.insertFlight(
                tbFlightCustContact.Text,
                tbFlightAirline.Text,
                tbFlightOrigin.Text,
                tbFlightDestination.Text,
                tbFlightNumber.Text,
                tbFlightSeat.Text,
                varDateTime,
                tbFlightAdult.Text,
                tbFlightChildren.Text,
                tbFlightInfant.Text,
                 varPaid);
            }
            tbFlightCustContact.Text =
                tbFlightAirline.Text =
                tbFlightOrigin.Text =
                tbFlightDestination.Text =
                tbFlightNumber.Text =
                tbFlightSeat.Text =
                
                tbFlightAdult.Text =
                tbFlightChildren.Text =
                tbFlightInfant.Text =
                 "";
            resetControls("view flights");
        }

        private void viewFlightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetControls("view flights"); devLogs("show bookings");
        }


        private void btSearchFlight_Click(object sender, EventArgs e)
        {
            resetControls("search flight query");
        }

        private void searchFlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetControls("search flight");
        }


        private string selectedColumnName;
        private int selectedRowID;

        private void dgRoomBookingsSummary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedColumnName = dgRoomBookingsSummary.Columns[0].Name;
            selectedRowID = (int)dgRoomBookingsSummary.CurrentRow.Cells[0].Value;
        }

        private void dgRoomBookingsSummary_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            selectedColumnName = dgRoomBookingsSummary.Columns[0].Name;
            selectedRowID = (int)dgRoomBookingsSummary.CurrentRow.Cells[0].Value;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (selectedColumnName != null && selectedRowID != 0)
            {

                if (selectedColumnName == "bookingID")
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this booking?", "Delete booking", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        mysqlConn.deleteBooking(Convert.ToString(selectedRowID));
                        DialogResult completeResult = MessageBox.Show("You have successfully deleted this booking", "Delete Successful", MessageBoxButtons.OK);
                        if (completeResult == DialogResult.OK)
                        {
                            resetControls("landing");
                        }
                    }
                 
                   
                }
                if (selectedColumnName == "custID")
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this customer?", "Delete customer", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        mysqlConn.deleteCustomer(Convert.ToString(selectedRowID));
                        DialogResult completeResult = MessageBox.Show("You have successfully deleted this customer", "Delete Successful", MessageBoxButtons.OK);
                        if (completeResult == DialogResult.OK)
                        {
                            resetControls("view customers");
                        }
                    }
                   
                   
                }
                if (selectedColumnName == "flightID")
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this flight?", "Delete flight", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        mysqlConn.deleteFlight(Convert.ToString(selectedRowID));
                        DialogResult completeResult = MessageBox.Show("You have successfully deleted this flight", "Delete Successful", MessageBoxButtons.OK);
                        if (completeResult == DialogResult.OK)
                        {
                            resetControls("view flights");
                        }
                    }
             
                   
                }
            }
          
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (selectedColumnName != null && selectedRowID != 0)
            {
                if (selectedColumnName == "flightID")
                {
                    devLogs(Convert.ToString(selectedRowID));
                    int id = (int)selectedRowID;
                    string query = string.Format("SELECT * FROM `tblflights` WHERE `flightID` = {0}", id);


                    DataRow dr = mysqlConn.qry(query).Tables[0].Rows[0];

                    tbFlightCustContact.Text = Convert.ToString(dr.Field<string>("custContact"));
                    tbFlightAirline.Text = Convert.ToString(dr.Field<string>("airLine"));
                    tbFlightOrigin.Text = Convert.ToString(dr.Field<string>("flightOrigin"));
                    tbFlightDestination.Text = Convert.ToString(dr.Field<string>("flightDestination"));
                    tbFlightNumber.Text = Convert.ToString(dr.Field<string>("flightNumber"));
                    tbFlightSeat.Text = Convert.ToString(dr.Field<string>("seatNumber"));
                    tbFlightAdult.Text = Convert.ToString(dr.Field<string>("adultCost"));
                    tbFlightChildren.Text = Convert.ToString(dr.Field<string>("childrenCost"));
                    tbFlightInfant.Text = Convert.ToString(dr.Field<string>("infantCost"));

                    bool paid;
                    if (Convert.ToString(dr.Field<string>("infantCost")) == "Y")
                    {
                        paid = true;
                    }
                    else
                    {
                        paid = false;
                    }

                    cbFlightPaid.Checked = paid;
                    dtFlightBook.ShowToday = true;

                    resetControls("edit flight");
                }
                if (selectedColumnName == "custID")
                {
                    devLogs(Convert.ToString(selectedRowID));
                    int id = (int)selectedRowID;
                    string query = string.Format("SELECT * FROM `tblcustomer` WHERE `custID` = {0}", id);


                    DataRow dr = mysqlConn.qry(query).Tables[0].Rows[0];

                    tbCustContact.Text = Convert.ToString(dr.Field<string>("custContact"));
                    tbCustEmail.Text = Convert.ToString(dr.Field<string>("custEmail"));
                    tbCustTel.Text = Convert.ToString(dr.Field<string>("custTel"));
                    tbCustNationality.Text = Convert.ToString(dr.Field<string>("custNationality"));
                    tbCustAdd1.Text = Convert.ToString(dr.Field<string>("custAddr1"));
                    tbCustAdd2.Text = Convert.ToString(dr.Field<string>("custAddr2"));
                    tbCustTownCity.Text = Convert.ToString(dr.Field<string>("custTownCity"));
                    tbCustPostcode.Text = Convert.ToString(dr.Field<string>("custPostcode"));

                    resetControls("edit customer");
                }
            }

        }

        private void btnEditFlightSave_Click(object sender, EventArgs e)
        {
            devLogs("update new flight");
            string varDateTime = dtFlightBook.SelectionRange.Start.ToString("yyyy-MM-dd") + " " + tbTime.Text + ":00"; ;
            string varPaid;
            if (cbFlightPaid.Checked == true) { varPaid = "Y"; }
            else { varPaid = "N"; }
            if (mysqlConn.connOpen() == true)
            {
                mysqlConn.updateFlight(
                selectedRowID.ToString(),
                tbFlightCustContact.Text,
                tbFlightAirline.Text,
                tbFlightOrigin.Text,
                tbFlightDestination.Text,
                tbFlightNumber.Text,
                tbFlightSeat.Text,
                varDateTime,
                tbFlightAdult.Text,
                tbFlightChildren.Text,
                tbFlightInfant.Text,
                 varPaid);
            }
            tbFlightCustContact.Text =
                tbFlightAirline.Text =
                tbFlightOrigin.Text =
                tbFlightDestination.Text =
                tbFlightNumber.Text =
                tbFlightSeat.Text =

                tbFlightAdult.Text =
                tbFlightChildren.Text =
                tbFlightInfant.Text =
                 "";
            DialogResult completeResult = MessageBox.Show("You have successfully updated this flight", "Update Successful", MessageBoxButtons.OK);
            if (completeResult == DialogResult.OK)
            {
                resetControls("view flights");
            }
           
        }

        private void lbCueSys_Click(object sender, EventArgs e)
        {

        }

        private void dgRoomBookingsSummary_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void btCustEdit_Click(object sender, EventArgs e)
        {
            devLogs("update customer");
            if (mysqlConn.connOpen() == true)
            {
                mysqlConn.updateCustomer(selectedRowID.ToString(), tbCustContact.Text, tbCustEmail.Text, tbCustTel.Text, tbCustNationality.Text, tbCustAdd1.Text, tbCustAdd2.Text, tbCustTownCity.Text, tbCustPostcode.Text);
            }
            tbCustContact.Text = tbCustEmail.Text = tbCustTel.Text = tbCustNationality.Text = tbCustAdd1.Text = tbCustAdd2.Text = tbCustTownCity.Text = tbCustPostcode.Text = "";
            DialogResult completeResult = MessageBox.Show("You have successfully updated this customer", "Update Successful", MessageBoxButtons.OK);
            if (completeResult == DialogResult.OK)
            {
                resetControls("view customers");
            }
            
        }







        ///// EVENTS END ///////////////////////////////////////////////////////////
    }
}
