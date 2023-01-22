// See https://aka.ms/new-console-template for more information

using BTree;

var head = new Element(2, null!, new List<int>(), new List<Element>());
for (int i = 1; i <= 20; i++)
{
    Console.WriteLine($"i = {i}");
    head.Add(i);
    head = Refresh(head);
    OutputTree(head, 0);
    Console.WriteLine("=====================================================");
}

void OutputTree(Element head, int level)
{
    Console.Write($"level = {level}     ");
    Console.WriteLine(string.Join(',', head.Keys));
    if(head.Children !=null && head.Children.Count > 0)
        foreach (var child in head.Children)
        {
            OutputTree(child, level+1);
        }
}

Element Refresh(Element head)
{
    return head.Parent == null ? head : Refresh(head.Parent);
}