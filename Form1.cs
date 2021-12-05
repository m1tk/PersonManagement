using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace guigui;

public partial class Form1 : Form {
    enum form_state {
        INITIAL,
        ADD,
        MODIFY,
        DELETE
    }

    private form_state curr_state;

    public  ComboBox list;
    private PersonTable table;

    public Button reload;
    public Label is_connected; 

    public Button Add;
    public Button Modify;
    public Button Delete;
    public Button Confirm;
    public Button Cancel;

    public Label name;
    public Label lname;
    public Label age;

    public TextBox Vname;
    public TextBox Vlname;
    public TextBox Vage;

    public ToolTip tips;

    // Time for erros display
    private System.Timers.Timer timer;
    public  Label error;

    public Form1() {
        tips        = new ToolTip();
        tips.Active = true;
        tips.AutoPopDelay = 4000;
        tips.InitialDelay = 600;

        list               = new ComboBox();
        list.DropDownStyle = ComboBoxStyle.DropDownList;
        list.Size          = new Size(540, 80);
        list.Location      = new Point(20, 60);
        this.table         = new PersonTable();

        this.table.Persons.Add(new Person(-1, "", "", 0));
        list.DataSource    = this.table.Persons;
        list.DisplayMember = "Name";
        list.ValueMember   = "Name";
        this.Controls.Add(list);
        this.list.DropDownClosed += new EventHandler(SelectedChanged);

        // reload button
        reload                       = new Button();
        reload.Size                  = new Size(25, 25);
        reload.Location              = new Point(560, 59);
        reload.BackgroundImage       = Image.FromFile(@"./reload.png");
        reload.BackgroundImageLayout = ImageLayout.Stretch;
        this.reload.Click           += new EventHandler(Reload_table);
        tips.SetToolTip(reload, "Refresh table");
        this.Controls.Add(reload);

        // Connection status
        is_connected                 = new Label();
        is_connected.Size            = new Size(0, 0);
        is_connected.Location        = new Point(585, 59);
        is_connected.BackgroundImage = Image.FromFile(@"./no_connection.png");
        is_connected.BackgroundImageLayout = ImageLayout.Stretch;
        tips.SetToolTip(is_connected, "Database unreachable");
        this.Controls.Add(is_connected);

        // Labels for text boxes
        name = new Label();
        name.Text = "First Name";
        name.Size = new Size (100, 40);
        name.Location = new Point(20, 150);
        this.Controls.Add(name);

        lname = new Label();
        lname.Text = "Last Name";
        lname.Size = new Size (100, 40);
        lname.Location = new Point(20, 190);
        this.Controls.Add(lname);
        
        age = new Label();
        age.Text = "Age";
        age.Size = new Size (100, 40);
        age.Location = new Point(20, 230);
        this.Controls.Add(age);

        // Text boxes

        this.Vname = new TextBox();
        Vname.Size = new Size (460, 60);
        Vname.Location = new Point(140, 145);
        this.Vname.AcceptsReturn = true;
        this.Vname.AcceptsTab = true;
        this.Vname.Multiline = false;
        this.Vname.Enabled = false;
        this.Controls.Add(this.Vname);

        this.Vlname = new TextBox();
        Vlname.Size = new Size (460, 60);
        Vlname.Location = new Point(140, 185);
        this.Vlname.AcceptsReturn = true;
        this.Vlname.AcceptsTab = true;
        this.Vlname.Multiline = false;
        this.Vlname.Enabled = false;
        this.Controls.Add(this.Vlname);

        this.Vage = new TextBox();
        Vage.Size = new Size (460, 60);
        Vage.Location = new Point(140, 225);
        this.Vage.AcceptsReturn = true;
        this.Vage.AcceptsTab = true;
        this.Vage.Multiline = false;
        this.Vage.Enabled = false;
        this.Controls.Add(this.Vage);
        
        // Setup button layout
        Add = new Button();
        Add.Size = new Size(100, 40);
        Add.Location = new Point(20, 400);
        Add.Text = "Add";
        this.Controls.Add(Add);
        this.Add.Click += new EventHandler(Add_element);

        Delete = new Button();
        Delete.Size = new Size(100, 40);
        Delete.Location = new Point(140, 400);
        Delete.Text = "Delete";
        this.Controls.Add(Delete);
        this.Delete.Click += new EventHandler(Delete_element);


        Modify = new Button();
        Modify.Size = new Size(100, 40);
        Modify.Location = new Point(260, 400);
        Modify.Text = "Modify";
        this.Controls.Add(Modify);
        this.Modify.Click += new EventHandler(Modify_element);

        Confirm = new Button();
        Confirm.Size = new Size(100, 40);
        Confirm.Location = new Point(380, 400);
        Confirm.Text = "Confirm";
        this.Controls.Add(Confirm);
        this.Confirm.Click += new EventHandler(Confirm_event);

        Cancel = new Button();
        Cancel.Size = new Size(100, 40);
        Cancel.Location = new Point(500, 400);
        Cancel.Text = "Cancel";
        this.Controls.Add(Cancel);
        this.Cancel.Click += new EventHandler(Cancel_event);


        timer = new System.Timers.Timer(5000);
        timer.SynchronizingObject = this;
        timer.Elapsed += new System.Timers.ElapsedEventHandler(clear_error_message);
        timer.AutoReset = false;

        error                 = new Label();
        error.Size            = new Size(540, 40);
        error.Location        = new Point(20, 450);
        error.ForeColor       = System.Drawing.Color.Red;
        error.Text            = "";
        this.Controls.Add(error);


        this.update_list();
        this.button_default_states();
        InitializeComponent();
    }

    public void clear_error_message(object sender, System.Timers.ElapsedEventArgs e) {
        this.error.Text = "";
    }

    private void update_error_message(String error) {
        if (this.timer.Enabled == true) {
            this.timer.Enabled = false;
        }
        this.error.Text    = error;
        this.timer.Enabled = true;
    }

    private void Reload_table(object sender, EventArgs e) {
        this.update_list();
    }

    private void update_list() {
        try {
            this.table.update_list();
            this.is_connected.Size = new Size(0, 0);
        } catch (Exception e) {
            this.is_connected.Size = new Size(25, 25);
            this.update_error_message(e.Message);
        }
    }

    private void SelectedChanged(object sender, EventArgs e) {
        if ((String)this.list.SelectedValue != "") {
            this.Add.Enabled    = true;
            this.Modify.Enabled = true;
            this.Delete.Enabled = true;
        } else {
            this.button_default_states();
            this.textboxes_state(false);
        }
    }

    private void button_default_states() {
        this.Add.Enabled     = true;
        this.Modify.Enabled  = false;
        this.Delete.Enabled  = false;
        this.Confirm.Enabled = false;
        this.Cancel.Enabled  = false;
        this.empty_textboxes();
    }

    private void empty_textboxes() {
        this.Vname.Text  = "";
        this.Vlname.Text = "";
        this.Vage.Text   = "";
    }

    private void modifier_state(bool state) {
        this.Add.Enabled     = state;
        this.Modify.Enabled  = state;
        this.Delete.Enabled  = state;
    }

    private void textboxes_state(bool state) {
        this.Vname.Enabled  = state;
        this.Vlname.Enabled = state;
        this.Vage.Enabled   = state;
    }

    private void Add_element(object sender, EventArgs e) {
        this.textboxes_state(true);
        this.modifier_state(false);
        this.curr_state      = Form1.form_state.ADD;
        this.list.Enabled    = false;
        this.Confirm.Enabled = true;
        this.Cancel.Enabled  = true;
    }

    private void Delete_element(object sender, EventArgs e) {
        this.textboxes_state(false);
        this.modifier_state(false);
        this.curr_state      = Form1.form_state.DELETE;
        this.list.Enabled    = true;
        this.Confirm.Enabled = true;
        this.Cancel.Enabled  = true;
    }

    private void Modify_element(object sender, EventArgs e) {
        this.textboxes_state(true);
        this.modifier_state(false);
        int ind = this.list.SelectedIndex;
        this.curr_state  = Form1.form_state.MODIFY;
        this.Vname.Text  = (String)this.table.Persons[ind].Name.Clone();
        this.Vlname.Text = (String)this.table.Persons[ind].Lname.Clone();
        this.Vage.Text   = (String)this.table.Persons[ind].Age.ToString();
        this.list.Enabled    = false;
        this.Confirm.Enabled = true;
        this.Cancel.Enabled  = true;
    }

    private bool verify_input(ref int age) {
        if (this.Vname.Text == "" || this.Vlname.Text == "" || this.Vage.Text == "") {
            this.update_error_message("All fields must be filled");
            return false;
        }
        if (!int.TryParse(this.Vage.Text, out age)) {
            this.update_error_message("Age must be a number");
            return false;
        }
        return true;
    }

    private void Confirm_event(object sender, EventArgs e) {
        switch(this.curr_state) {
            case Form1.form_state.ADD: {
                int age = 0;
                if (this.verify_input(ref age)) {
                    this.add_elem(this.Vname.Text, this.Vlname.Text, age);
                    this.button_default_states();
                    this.textboxes_state(false);
                    this.list.Enabled = true;
                    this.Modify.Enabled = true;
                    this.Delete.Enabled = true;
                    this.list.SelectedIndex = this.table.Persons.Count-1;
                }
                break;
            }
            case Form1.form_state.MODIFY: {
                int age = 0;
                if (this.verify_input(ref age)) {
                    this.verify_input(ref age);
                    int selected = this.list.SelectedIndex;
                    this.modify_selected_elem(this.list.SelectedIndex, this.Vname.Text, this.Vlname.Text, age);
                    this.empty_textboxes();
                    this.modifier_state(true);
                    this.textboxes_state(false);
                    this.Confirm.Enabled = false;
                    this.Cancel.Enabled  = false;
                    this.list.Enabled    = true;
                    this.list.SelectedIndex = selected;
                }
                break;
            }
            case Form1.form_state.DELETE: {
                this.delete_elem(this.list.SelectedIndex);
                this.button_default_states();
                this.textboxes_state(false);
                this.list.SelectedIndex = 0;
                break;
            }
            default:
                break;
        }
    }
    
    private void add_elem(String name, String lname, int age) {
        try {
            this.table.Add(name, lname, age);
            this.is_connected.Size = new Size(0, 0);
        } catch (Exception e) {
            this.is_connected.Size = new Size(25, 25);
            this.update_error_message(e.Message);
        }
    }

    private void modify_selected_elem(int ind, String name, String lname, int age) {
        try {
            this.table.Modify(ind, name, lname, age);
            this.is_connected.Size = new Size(0, 0);
        } catch (Exception e) {
            this.is_connected.Size = new Size(25, 25);
            this.update_error_message(e.Message);
        }
    }

    private void delete_elem(int ind) {
        try {
            this.table.Delete(ind);
            this.is_connected.Size = new Size(0, 0);
        } catch (Exception e) {
            this.is_connected.Size = new Size(25, 25);
            this.update_error_message(e.Message);
        }
    }

    private void Cancel_event(object sender, EventArgs e) {
        switch(this.curr_state) {
            case Form1.form_state.ADD: {
                this.button_default_states();
                this.textboxes_state(false);
                this.list.Enabled = true;
                break;
            }
            case Form1.form_state.MODIFY: {
                this.empty_textboxes();
                this.modifier_state(true);
                this.textboxes_state(false);
                this.Confirm.Enabled = false;
                this.Cancel.Enabled  = false;
                this.list.Enabled    = true;
                break;
            }
            case Form1.form_state.DELETE: {
                this.button_default_states();
                this.textboxes_state(false);
                break;
            }
            default:
                break;
        }
    }
}
