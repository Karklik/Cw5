using Cw5.Models;
using System.Collections.Generic;

namespace Cw5.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents(string orderBy);
        public Student GetStudent(string indexNumber);
        public int CreateStudent(Student student);
        public int UpdateStudent(string indexNumber, Student student);
        public int DeleteStudent(string indexNumber);
        public Enrollment GetStudentEnrollment(string indexNumber);
    }
}
