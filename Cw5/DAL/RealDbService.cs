using Cw5.Models;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Cw5.DAL
{
    public class RealDbService : IDbService
    {
        private readonly string connectionString = "Data Source=db-mssql;Initial Catalog=s16556;Integrated Security=True";
        public int CreateStudent(Student student)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "INSERT INTO Student " +
                "VALUES(@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)"
            };
            command.Parameters.AddWithValue("indexNumber", student.IndexNumber);
            command.Parameters.AddWithValue("firstName", student.FirstName);
            command.Parameters.AddWithValue("lastName", student.LastName);
            command.Parameters.AddWithValue("birthDate", student.BirthDate);
            command.Parameters.AddWithValue("idEnrollment", student.IdEnrollment);
            connection.Open();
            return command.ExecuteNonQuery();
        }

        public int DeleteStudent(string indexNumber)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "DELETE FROM Student WHERE IndexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);
            connection.Open();
            return command.ExecuteNonQuery();
        }

        public Student GetStudent(string indexNumber)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * FROM Student WHERE IndexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);
            connection.Open();
            var dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                var student = new Student
                {
                    IndexNumber = dataReader["IndexNumber"].ToString(),
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    BirthDate = dataReader["BirthDate"].ToString(),
                    IdEnrollment = IntegerType.FromObject(dataReader["IdEnrollment"])
                };
                return student;
            }
            return null;
        }

        public Enrollment GetStudentEnrollment(string indexNumber)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT Enrollment.IdEnrollment, Semester, StartDate, Name " +
                "FROM Student " +
                "INNER JOIN Enrollment ON Student.IdEnrollment = Enrollment.IdEnrollment " +
                "INNER JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy " +
                "WHERE IndexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);
            connection.Open();
            var dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                var enrollment = new Enrollment
                {
                    IdEnrollment = IntegerType.FromObject(dataReader["IdEnrollment"]),
                    Semester = dataReader["Semester"].ToString(),
                    StartDate = dataReader["StartDate"].ToString(),
                    Name = dataReader["Name"].ToString(),
                };
                return enrollment;
            }
            return new Enrollment();
        }

        public IEnumerable<Student> GetStudents(string orderBy)
        {
            if (orderBy == null)
                orderBy = "IndexNumber";
            List<Student> students = new List<Student>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand()
            {
                Connection = connection,
                CommandText = $"SELECT * FROM Student ORDER BY {orderBy}"
            };
            connection.Open();
            var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                var student = new Student
                {
                    IndexNumber = dataReader["IndexNumber"].ToString(),
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    BirthDate = dataReader["BirthDate"].ToString(),
                    IdEnrollment = IntegerType.FromObject(dataReader["IdEnrollment"])
                };
                students.Add(student);
            }
            return students;
        }

        public int UpdateStudent(string indexNumber, Student student)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "UPDATE Student " +
                "SET IndexNumber = @newIndexNumber, FirstName = @firstName, " +
                "LastName = @lastName, BirthDate = @birthDate, " +
                "IdEnrollment = @idEnrollment " +
                "WHERE IndexNumber = @oldIndexNumber"
            };
            command.Parameters.AddWithValue("newIndexNumber", student.IndexNumber);
            command.Parameters.AddWithValue("firstName", student.FirstName);
            command.Parameters.AddWithValue("lastName", student.LastName);
            command.Parameters.AddWithValue("birthDate", student.BirthDate);
            command.Parameters.AddWithValue("idEnrollment", student.IdEnrollment);
            command.Parameters.AddWithValue("oldIndexNumber", indexNumber);
            connection.Open();
            return command.ExecuteNonQuery();
        }
    }
}
