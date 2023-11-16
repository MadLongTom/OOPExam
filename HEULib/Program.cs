// See https://aka.ms/new-console-template for more information

Library lib = new("南通大街145号", "哈工程图书馆");
lib.AddBook("A Song of Ice and Fire", "神秘的作者", "神秘的出版社", DateTime.Now.Year.ToString(), 2);
Console.WriteLine(lib);
lib.Lend("A Song of Ice and Fire", "小明");
Console.WriteLine(lib);
lib.Lend("A Song of Ice and Fire", 3, "小明");
lib.Return("A Song of Ice and Fire", "小明");
Console.WriteLine(lib);
lib.SearchBook("A Song of Ice and Fire");
lib.SearchBook("SearchTest");

public class LendStatus
{
    public string? LendName;
    public DateTime Due;
    public int Count;
    public override string ToString()
    {
        return $"借阅者：{LendName}，借阅数目：{Count}，约定还书日期：{Due}";
    }
}

public class Book
{
    public readonly string Name, Author, Publisher, PublishYear;
    private readonly Guid guid;
    public int Storage;
    public List<LendStatus> status = new();
    public Book(string name, string author, string publisher, string publishYear, int storage)
    {
        guid = Guid.NewGuid();
        Storage = storage;
        Name = name;
        Author = author;
        Publisher = publisher;
        PublishYear = publishYear;
    }
    public bool CanBeLent(int count) => Storage > count - 1;
    public override string ToString()
    {
        return $"GUID：{guid}" + Environment.NewLine +
               $"名称：{Name}" + Environment.NewLine +
               $"作者：{Author}" + Environment.NewLine +
               $"出版社：{Publisher}" + Environment.NewLine +
               $"出版年份：{PublishYear}" + Environment.NewLine +
               $"藏书数量：{Storage}" + Environment.NewLine +
               $"借阅状态：" + Environment.NewLine + string.Join(Environment.NewLine, status) +
               Environment.NewLine;
    }
}

public class Library
{
    private List<Book> books = new();
    public string Name;
    public string Address;
    public int BookCount { get => books.Sum(b => b.Storage); }
    public Library(string address, string name)
    {
        Address = address;
        Name = name;
    }
    public void AddBook(string name, string author, string publisher, string publishYear, int storage)
    {
        books.Add(new Book(name, author, publisher, publishYear, storage));
    }
    public void SearchBook(string book)
    {
        Book? selectedBook;
        Console.WriteLine((selectedBook = books.Where(b => b.Name == book).FirstOrDefault()) != default(Book) ? selectedBook : $"查无此书：{book}");
    }
    public bool Lend(string name, int count, string lender)
    {
        Book? selectedBook;
        if ((selectedBook = books.Where(b => b.Name == name).FirstOrDefault()) == default(Book))
        {
            Console.WriteLine($"查无此书：{name}");
            return false;
        }
        if (!selectedBook!.CanBeLent(count))
        {
            Console.WriteLine("库存不足！");
            return false;
        }
        selectedBook.Storage -= count;
        selectedBook.status.Add(new LendStatus { LendName = lender, Count = count, Due = DateTime.Now + TimeSpan.FromDays(15) });
        return true;
    }
    public bool Lend(string name, string lender) => Lend(name, 1, lender);
    public bool Return(string name, int count, string lender)
    {
        Book? selectedBook;
        if ((selectedBook = books.Where(b => b.Name == name).FirstOrDefault()) == default(Book))
        {
            Console.WriteLine($"查无此书：{name}");
            return false;
        }
        if (!selectedBook.status.Any(s => s.LendName == lender))
        {
            Console.WriteLine("没有此人的借阅记录！");
            return false;
        }
        LendStatus status = selectedBook.status.Where(s => s.LendName == lender).First();
        if (status.Due < DateTime.Now)
        {
            Console.WriteLine("超过借阅期限");
        }
        selectedBook.Storage += count;
        status.Count -= count;
        if (status.Count == 0)  selectedBook.status.Remove(status); 
        return true;
    }
    public bool Return(string name, string lender) => Return(name, 1, lender);
    public override string ToString()
    {
        return $"{Name}" + Environment.NewLine +
               $"地址：{Address}" + Environment.NewLine +
               $"藏书：" + Environment.NewLine + string.Join(Environment.NewLine, books)
               + Environment.NewLine;
    }
}