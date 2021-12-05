using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace guigui;

public class Person {
    public int id { get; }
    public String Name { get; }
    public String Lname { get;  }
    public int Age { get; }

    public Person(int mid, String mname, String mlname, int mint) {
        this.id    = mid;
        this.Name  = (String)mname.Clone();
        this.Lname = (String)mlname.Clone();
        this.Age   = mint;
    }
}

public class PersonTable {
    private MySqlConnection con;
    private MySqlDataAdapter adapter;

    public BindingList<Person> Persons;

    public PersonTable() {
        MySqlConnection MySQLConnection = new MySqlConnection();
        this.adapter = new MySqlDataAdapter();
        this.con     =  new MySqlConnection("server=192.168.188.131;userid=persongui;password=pass1;database=person");
        this.Persons = new BindingList<Person>();
    }

    private void connect() {
        try {
            this.con.Open();
        } catch (MySqlException e) {
            throw new Exception(e.Message);
        }
    }

    private void close() {
        try {
            this.con.Close();
        } catch (MySqlException e) {
            throw new Exception("Error closing connection to database");
        }
    }

    private int ExecuteNonQuery(MySql.Data.MySqlClient.MySqlCommand cmd) {
        int ret;
        try {
            ret = cmd.ExecuteNonQuery();
        } catch (MySql.Data.MySqlClient.MySqlException ex) {
            throw new Exception("Error sending query to database");
        }
        this.close();
        return ret;
    }

    public int Delete(int ind) {
        this.connect();
        MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
        cmd.Connection  = this.con;
        cmd.CommandText = "DELETE FROM person WHERE id = @id";
        cmd.Parameters.AddWithValue("@id",  this.Persons[ind].id);
        int ret = this.ExecuteNonQuery(cmd);
        if (ret == 1)  {
            this.Persons.RemoveAt(ind);
        }
        return ret;
    }

    public int Add(String mname, String mlname, int mage) {
        this.connect();
        MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
        cmd.Connection  = this.con;
        cmd.CommandText = "INSERT INTO person (name, lname, age) VALUES(@name, @lname, @age)";
        cmd.Parameters.AddWithValue("@name", mname);
        cmd.Parameters.AddWithValue("@lname", mlname);
        cmd.Parameters.AddWithValue("@age", mage);
        int ret = this.ExecuteNonQuery(cmd);
        if (ret == 1)  {
            this.Persons.Add(new Person((int)cmd.LastInsertedId, mname, mlname, mage));
        }
        return ret;
    }

    public int Modify(int ind, String mname, String mlname, int mage) {
        this.connect();
        MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
        cmd.Connection  = this.con;
        cmd.CommandText = "UPDATE person SET name = @name, lname = @lname, age = @age where id = @id";
        cmd.Parameters.AddWithValue("@name", mname);
        cmd.Parameters.AddWithValue("@lname", mlname);
        cmd.Parameters.AddWithValue("@age", mage);
        cmd.Parameters.AddWithValue("@id", this.Persons[ind].id);
        int ret = this.ExecuteNonQuery(cmd);
        if (ret == 1)  {
            this.Persons[ind] = new Person((int)cmd.LastInsertedId, mname, mlname, mage);
        }
        return ret;
    }

    public void update_list() {
        if (this.Persons.Count != 1) {
            Person first = this.Persons[0];
            this.Persons.Clear();
            this.Persons.Add(first);
        }
        this.connect();
        MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
        cmd.Connection  = this.con;
        cmd.CommandText = "SELECT id, name, lname, age FROM person";
        using MySqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read()) {
            this.Persons.Add(new Person(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3)));
        }
        this.close();
    }
}
