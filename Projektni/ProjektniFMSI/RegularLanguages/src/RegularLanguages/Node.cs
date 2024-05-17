namespace RegularLanguages
{
    /*
        The Node class is a helper class
        used by the IRegular Interface
        To create a parsing tree 
        for regular expresions.
        Its members a private.
        They all have getters but not setters.
        The mebers are:
        
        -Data string either used to represent
        an operator i.e. | () * •.
        or to represent a symbol in the
        regular expresion all the possible
        symbols are all other possible
        charcters not used as operators.
        
        -Node left and right nullable:
         and ar used as the left and right
         child Nodes of a given Node.
         Operators have child nodes
         while regular symbols dont.
    */
    public class Node
    {
        private Node? left, right;
        private string data;

        /*
            Private constructor used to 
            intilize the data string
            by internal static functions.
            Nodes are not ment to be created
            outside the context of those
            static functions.
        */
        private Node(string data)
        {

            this.data = data;
        }
        /*
            returns the data string of a given Node.
        */
        public string getData() { return this.data;}

        /*
            returns the left child of a given Node.
        */
        public Node? getLeft() { return this.left;}
        
        /*
            returns the right child of a given Node.
        */
        public Node? getRight() { return this.right;}
        
        /*
            A helper function used to evaluet the stacks
            used by the tree function.
            It exspects no operators just operands on the
            stack and treats the as concats. 
        */
        private static  Node EvaluateStack(Stack<Node> s)
        {
            if (s.Count == 1)
                return s.Pop();
            else
            {
                Node t = new Node("•");
                t.right = s.Pop();
                t.left = EvaluateStack(s);
                return t;
            }

        }


    /*
        The tree function is the function
        that acctualy creats the parsing tree.
        It does so by going over the string
        with an ref int which detirmens the 
        current postion on the string
        representing the regular expresion.

        If there is no operator between
        operands it makes a Node with no
        children containg the symbol as that
        and pushes the Node on to the stack.
        
        If the operator is * it takes the 
        first expresion on the stack and
        and creats a Node with the * operator
        whos left child is the expresion on 
        the stack and returns the newly created
        Node on to the stack.

        If the operator is | it evluates the stack
        concating all the expresions on it thus 
        creating a subtree the subtree becomes
        the left child of the Node.
        The data of the Node is |
        The Right child is the expresion to the left
        which is got by recursivly calling the
        tree function and moving the ref int by one.
        Once the recursive call is over. 
        It returns the created Node.

        If the operator is a ( it  calls
        the tree function recursivly 
        moving the refrence one 
        to creat a subtree for the expresion
        between the brackets and pushes
        the result on to the stack.

        If the operator is a ) it Evaluets
        the stack and returns the 
        subtree created from the evaluation of the
        stack.

    */
    private static Node Tree(string RI, ref int i)
        {
            Stack<Node> s = new Stack<Node>();
            for (; i < RI.Length; i++)

                if (RI[i] == '(')
                {
                    i++;
                    Node sub_expresion = Tree(RI, ref i);
                    s.Push(sub_expresion);
                }
                else if (RI[i] == ')')
                {
                    return EvaluateStack(s);
                }
                else if (RI[i] == '|')
                {

                    Node temp = new Node("|");
                    temp.left = EvaluateStack(s);
                    i++;
                    temp.right = Tree(RI, ref i);
                    return temp;
                }
                else if (RI[i] == '*')
                {
                    Node temp = new Node("*");
                    temp.left = s.Pop();
                    s.Push(temp);
                }
                else
                {
                    Node temp = new Node(RI[i] + "");
                    s.Push(temp);
                }


            return EvaluateStack(s);
        }
    /*
     The public function used to create
     a parsing tree it does so by calling
     the tree function with a int ref equaling 0, 
     and the Regular Expresion string.
    */
    public static Node ParsingTree(string RE) 
    {
        int i=0;    
        return Tree(RE,ref i);
    }

    }

  

}