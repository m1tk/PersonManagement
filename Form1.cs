using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace guigui;

public class Person {
    public String Name { get; set; }
    public String Lname { get; set; }
    public int Age { get; set; }

    public Person(String mname, String mlname, int mint) {
        this.Name  = (String)mname.Clone();
        this.Lname = (String)mlname.Clone();
        this.Age   = mint;
    }
}

public partial class Form1 : Form {
    enum form_state {
        INITIAL,
        ADD,
        MODIFY,
        DELETE
    }

    private form_state curr_state;

    public  ComboBox list;
    private BindingList<Person> Members;

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

    public Form1() {

        list = new ComboBox();
        list.DropDownStyle = ComboBoxStyle.DropDownList;
        list.Size = new Size(580, 80);
        list.Location = new Point(20, 60);
        this.Members = new BindingList<Person>();
        this.Members.Add(new Person("", "", 0));
        list.DataSource = this.Members;
        list.DisplayMember = "Name";
        list.ValueMember = "Name";
        this.Controls.Add(list);
        this.list.DropDownClosed += new EventHandler(SelectedChanged);

        this.Members.Add(new Person("1loo", "1", 1));

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

        this.button_default_states();
        InitializeComponent();
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
        this.Vname.Text  = (String)this.Members[ind].Name.Clone();
        this.Vlname.Text = (String)this.Members[ind].Lname.Clone();
        this.Vage.Text   = (String)this.Members[ind].Age.ToString();
        this.list.Enabled    = false;
        this.Confirm.Enabled = true;
        this.Cancel.Enabled  = true;
    }

    private bool verify_input(ref int age) {
        if (this.Vname.Text == "" || this.Vlname.Text == "" || this.Vage.Text == "") {
            MessageBox.Show("All fields must be filled");
            return false;
        }
        if (!int.TryParse(this.Vage.Text, out age)) {
            MessageBox.Show("Age must be a number");
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
                    this.list.SelectedIndex = this.Members.Count-1;
                }
                break;
            }
            case Form1.form_state.MODIFY: {
                int age = 0;
                if (this.verify_input(ref age)) {
                    this.verify_input(ref age);
                    this.modify_selected_elem(this.list.SelectedIndex, this.Vname.Text, this.Vlname.Text, age);
                    this.empty_textboxes();
                    this.modifier_state(true);
                    this.textboxes_state(false);
                    this.Confirm.Enabled = false;
                    this.Cancel.Enabled  = false;
                    this.list.Enabled    = true;
                    this.list.SelectedIndex = this.Members.Count-1;
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
        this.Members.Add(new Person(name, lname, age));
    }

    private void modify_selected_elem(int ind, String name, String lname, int age) {
        this.Members[ind] = new Person(name, lname, age);
    }

    private void delete_elem(int ind) {
        this.Members.RemoveAt(ind);
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
