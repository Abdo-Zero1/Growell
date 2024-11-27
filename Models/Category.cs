using static System.Net.Mime.MediaTypeNames;

namespace Models
{
    public class Category
    {
       
            public int CategoryID { get; set; }
            public string Name { get; set; }

            // العلاقة مع جدول الاختبارات
            public ICollection<Test> Tests { get; set; }

    }
}
