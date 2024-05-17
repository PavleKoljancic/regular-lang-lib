// See https://aka.ms/new-console-template for more information
using parser;

void PrintTree(Node tree,bool b)
{
    if (tree != null)
    {
        if (b)
        {
            Console.Write("(");
           
        }
        PrintTree(tree.left,tree.data=="*");
        if (tree.data != "concat") Console.Write(tree.data);
        PrintTree(tree.right,tree.data=="*");
        if (b)
        {
            
            
            Console.Write(")");
        }
    }
}

Node EvaluateStack(Stack<Node> s)
{
    if (s.Count == 1)
        return s.Pop();
    else
    {
        Node t = new Node("concat");
        t.right = s.Pop();
        t.left = EvaluateStack(s);
        return t;
    }

}

Node tree(string RE, ref int i)
{
    Stack<Node> s = new Stack<Node>();
    for (; i < RE.Length; i++)

        if (RE[i] == '(')
        {
            i++;
            Node sub_expresion = tree(RE, ref i);
            s.Push(sub_expresion);
        }
        else if (RE[i] == ')')
        {
            return EvaluateStack(s);
        }
        else if (RE[i] == '|')
        {

            Node temp = new Node("|");
            temp.left = EvaluateStack(s);
            i++;
            temp.right = tree(RE, ref i);
            return temp;
        }
        else if (RE[i] == '*')
        {
            Node temp = new Node("*");
            temp.left = s.Pop();
            s.Push(temp);
        }
        else
        {
            Node temp = new Node(RE[i] + "");
            s.Push(temp);
        }


    return EvaluateStack(s);
}
