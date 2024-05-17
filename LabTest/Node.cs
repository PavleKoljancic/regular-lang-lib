namespace LabTest
{
    public class Node
    {
        Token data;
        Node? left;
        Node? right;
        public Node(Token data) {this.data= data;}
        public void setLeft(Node? left) { this.left= left;}
        public void setRight(Node? right) { this.right= right;}
        public Node? getLeft() { return left;}
        public Node? getRight() { return right;}
    }
}