namespace BTree;

public class Element
{
    /// <summary>
    /// Максимальное количество элементов в элементе дерева(ранг)
    /// </summary>
    private readonly int _t;

    private int MaxKeysCount => 2 * _t - 1;

    /// <summary>
    /// Ссылка на родительский элемент(узел)
    /// </summary>
    public Element? Parent { get; set; }

    /// <summary>
    /// Ссылки на потомков(узлы или листы)
    /// </summary>
    public List<Element> Children { get; set; }

    /// <summary>
    /// Список ключей текущего элемента
    /// </summary>
    public List<int> Keys { get; set; }

    public Element(int t, Element parent, List<int> keys, List<Element> children)
    {
        _t = t;
        Parent = parent;
        Keys = keys;
        Children = children;
    }

    /// <summary>
    /// Добавляет новый ключ в поддерево текущего элемента
    /// </summary>
    /// <param name="newKey"></param>
    public bool Add(int newKey)
    {
        if (Keys.Contains(newKey))
        {
            Console.WriteLine("Ключ уже существует!");
            return false;
        }

        //Если элемент является узлом
        if (!IsLeaf)
        {
            //Определяем интервал для нового элемента
            var i = 0;
            while (true)
            {
                if (newKey < Keys[i])
                {
                    break;
                }

                i++;
                if (i >= Keys.Count || i >= MaxKeysCount) break;
            }

            //Добавляем ключ к дереву потомков 
            Children[i].Add(newKey);
        }
        else
        {
            Keys.Add(newKey);
            Keys.Sort();
            //Если лист переполнен делим пополам и вместе с медианным ключом передаём родителю
            if (Keys.Count == MaxKeysCount)
            {
                //Если элемент корневой, то создаем пустой элемент, будущий корень дерева
                Parent ??= new Element(_t, null!, new List<int>(), new List<Element>(){this});

                var medianKey = Keys[_t - 1];
                var leftKeys = Keys.GetRange(0, _t - 1);
                var rightKeys = Keys.GetRange(_t, _t - 1);
                Keys = leftKeys;
                var newChild = new Element(_t, Parent, rightKeys, null!);

                Parent.Update(medianKey, newChild);
            }
        }

        return true;
    }

    public bool Update(int key, Element newChild)
    {
        Keys.Add(key);
        Keys.Sort();
        Children.Add(newChild);
        Children = Children.OrderBy(x => x.Keys[0]).ToList();

        //Если узел переполнен делим пополам и вместе с медианным ключом передаём родителю
        if (Keys.Count == MaxKeysCount)
        {
            Parent ??= new Element(_t, null!, new List<int>(), new List<Element>(){this});
            
            var medianKey = Keys[_t - 1];
            var leftKeys = Keys.GetRange(0, _t - 1);
            var leftChildren = Children.GetRange(0, _t);
            
            var rightKeys = Keys.GetRange(_t, _t - 1);
            var rightChild = new Element(_t, Parent, rightKeys, Children = Children.GetRange(_t, _t));
            rightChild.Children.ForEach(x => x.Parent = rightChild);

            Keys = leftKeys;
            Children = leftChildren;
            
            Parent.Update(medianKey, rightChild);
        }

        return true;
    }

    /// <summary>
    /// Определяет является ли элемент листом или узлом
    /// </summary>
    private bool IsLeaf => Children == null || Children.Count == 0;
}