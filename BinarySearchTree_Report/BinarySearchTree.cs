using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree_Report
{
    internal class BinarySearchTree<T> where T : IComparable<T> // 비교가 가능한 상태 만들어주기
    {
        // 노드 기반 비선형 구조를 갖고 있으니 노드를 먼저 생성해준다.
        private Node root;

        public BinarySearchTree()   // 처음 상태는 아무것도 없는 상태라 null로 초기화
        {
            this.root = null;
        }

        public void Add(T item)     // 추가
        {
            Node newNode = new Node(item, null, null, null);    // 노드에 부모null, 왼쪽/오른쪽 자식을 null로 일단 데이터 없는 상태로 시작한다

            if (root == null)           // 완전 초기상태로 바로 데이터가 들어가면 됨(아무것도 없는 상태)                
            {
                root = newNode;
                return;
            }

            Node current = root;
            while (current != null)     // 들어온 데이터가 null이 아닌경우
            {
                if (item.CompareTo(current.item) < 0)   // 부모와 새로운 데이터와 비교하여 새로운 데이터가 작은경우
                {
                    if (current.left != null)           // 왼쪽에 자식이 있을 경우 
                    {
                        current = current.left;         // 왼쪽 자식의 자식과 비교를 하기위해 변수를 대입하여 반복해준다
                    }
                    else                                // 왼쪽 자식이 없는 경우
                    {
                        current.left = newNode;         // 왼쪽 자식자리를 새롭게 만들고
                        newNode.parent = current;       // 해당 자리에 부모로 만들어준다
                        return;
                    }
                }
                else if (item.CompareTo(current.item) > 0) // 부모와 새로운 데이터와 비교하여 새로운 데이터가 더 큰경우
                {
                    if (current.right != null)             // 오른쪽 자식이 있을 경우
                    {
                        current = current.right;           // 오른쪽 자식의 자식과 비교를 하기위해 변수를 대입하여 반복해준다
                    }
                    else                                   // 오른쪽에 자식이 없을 경우
                    {
                        current.right = newNode;           // 오른쪽 자식자리를 새롭게 만들고
                        newNode.parent = current;          // 해당 자리에 부모로 만들어준다
                        return;
                    }
                }
                else                // 동일한 값이 들어와 해당 자리와 중복될 경우
                {
                    return;         // 아무것도 안하도록 바로 반환해준다
                }
            }
        }

        public bool Remove(T item)  // 삭제
        {
            if (root == null)       // 만약 데이터가 없을땐 false로 반환 
                return false;

            Node findNode = FindNode(item); // 탐색의 통해 왼쪽과 오른쪽으로 찾는 작업을 진행함
            if (findNode == null)           // 찾는 데이터가 없을 경우
            {
                return false;               // false로 반환한다
            }
            else                            // 찾는 데이터가 있을 경우
            {
                EraseNode(findNode);        // 노드지우기 함수로 해당 데이터를 삭제시킨다
                return true;
            }
        }

        private Node FindNode(T item)       // 노드탐색함수
        {
            if (root == null)               // 데이터가 아예 없는 경우 null로 반환
                return null;

            Node current = root;            
            while (current != null)         // 데이터가 null이 아닌경우
            {
                if (item.CompareTo(current.item) < 0)       // 부모와 비교해서 작은경우 왼쪽으로 이동
                {
                    current = current.left;
                }
                else if (item.CompareTo(current.item) > 0)  // 부모와 비교해서 클경우 오른쪽으로 이동
                {
                    current = current.right;
                }
                else                                        // 비교대상이 없을경우 바로 그값을 반환
                {
                    return current;
                }
            }

            return null;
        }

        private void EraseNode(Node node)                   // 노드삭제
        {
            if (node.HasNoChild)                            // 자식이 둘다 없을 경우
            {
                if (node.IsLeftChild)                       // 부모에 왼쪽에 연결되어 있을경우
                {
                    node.parent.left = null;                // 삭제대상과 연결된 왼쪽노드를 삭제한다
                }
                else if (node.IsRightChild)                 // 부모에 오른쪽에 연결되어 있을경우
                {
                    node.parent.right = null;               // 삭제대상과 연결된 오른쪽노드를 삭제한다
                }
                else // if (node.IsRootNode)                // 연결대상이 없을경우
                {
                    root = null;                            // 해당 데이터 삭제
                }

            }

            else if (node.HasLeftChild || node.HasRightChild)   // 왼쪽, 오른쪽 자식이 둘중 하나만 있을경우
            {
                Node parent = node.Parent;          
                Node child = node.HasLeftChild ? node.left : node.right;    // 왼쪽이 ture일 경우 왼쪽 아닐경우 오른쪽으로 자식변수에 대입

                if (node.IsLeftChild)                           // 부모에 왼쪽에 연결되어 있을경우
                {
                    parent.left = child;                        // 삭제대상의 왼쪽 자식과 부모과 연결된 왼쪽 노드를 연결
                    child.Parent = parent;                      // 왼쪽자식이 삭제대상 대신해서 부모로 자리 바꿈
                                                                // 연결이 사라지면 자동으로 해당 데이터는 삭제 된다
                }
                else if (node.IsRightChild)                     // 부모에 오른쪽에 연결되어 있을경우
                {
                    parent.Right = child;                       // 삭제대상의 오른쪽 자식과 부모과 연결된 오른쪽 노드를 연결
                    child.Parent = parent;                      // 오른쪽자식이 삭제대상 대신해서 부모로 자리 바꿈
                }                                               // 연결이 사라지면 자동으로 해당 데이터는 삭제 된다

                else // if (node.IsRootNode)                    // 부모가 없을 경우, 자식과 자리 변경후 삭제
                {
                    root = child;                               
                    child.Parent = null;
                }

            }
            
            else // if (node.HasBothChild)                      // 자식이 둘다 있을 경우
                                                                // 왼쪽자식(작은값) 루트에서 왼쪽자식의 오른쪽자식(큰값)과 자리를 교체한뒤 삭제해도 트리구조에 오류를 주지 않는다
            {
                Node replaceNode = node.left;                   // 왼쪽자식 변수대입
                while (replaceNode.right != null)               // 왼쪽자식의 오른쪽자식이 null이 될때 까지 반복
                {
                    replaceNode = replaceNode.right;            // 오른쪽 값으로 교체
                }
                node.item = replaceNode.item;                   // 자리 교체
                EraseNode(replaceNode);                         // 해당 데이터 삭제
            }
        }

        public bool TryGetValue(T item, out T outValue)        // 데이터 탐색 호출
        {
            if (root == null)                                  // 데이터가 전체 아예 없을 경우
            {
                outValue = default(T);                         // 자료형의 기본값 반환
                return false;
            }

            Node findNode = FindNode(item);                    // 찾는 데이터 노드안에 값 변수로 대입
            if (findNode == null)                              // 찾는 값이 없을 경우
            {
                outValue = default(T);                         // 자료형의 기본값 반환
                return false;
            }
            else //if (findNode != null)                       // 해당 데이터를 찾은 경우
            {
                outValue = findNode.item;                      // 해당 값 호출
                return true;
            }
        }

        
        public void Print()                                    // 전체 데이터 정렬된 상태로 출력
        {
            Print(root);
        }


        public void Print(Node node)                          // 출력 함수 구현
        {
            if (node.left != null) Print(node.left);          // 노드의 왼쪽이 null이 아닌경우 호출 (ex. 1(왼쪽노드에 있는 데이터)  2(해당) 3(오른쪽노드에 있는 데이터)
            Console.WriteLine(node.left);                     // 왼쪽에 있던 데이터 출력
            if (node.right != null) Print(node.right);        // 노드의 오른쪽이 null이 아닌경우 호출
        }

        public class Node                                     // 노드 구성
        {
            internal T item;                                  // 값
            internal Node parent;                             // 부모
            internal Node left;                               // 왼쪽
            internal Node right;                              // 오른쪽

            public Node(T item, Node parent, Node left, Node right)
            {
                this.item = item;
                this.parent = parent;
                this.left = left;
                this.right = right;
            }
            // 읽고 쓰기 다 가능함
            public T Item { get { return item; } set { item = value; } }
            public Node Parent { get { return parent; } set { parent = value; } }
            public Node Left { get { return left; } set { left = value; } }
            public Node Right { get { return right; } set { right = value; } }

            // 삭제대상 데이터의 부모과점
            public bool IsRootNode { get { return parent == null; } }                           // 부모가 없을경우
            public bool IsLeftChild { get { return parent != null && parent.left == this; } }   // 대상이 부모의 왼쪽에 연결된 상태
            public bool IsRightChild { get { return parent != null && parent.right == this; } } // 대상이 부모의 오른쪽에 연결된 상태

            // 자식의 유무에 대한 조건함수를 만들어둔다
            public bool HasNoChild { get { return left == null && right == null; } }            // 자식이 둘다 없을때
            public bool HasLeftChild { get { return left != null && right == null; } }          // 왼쪽 자식만 있을때
            public bool HasRightChild { get { return left == null && right != null; } }         // 오른쪽 자식만 있을때
            public bool HasBothChild { get { return left != null && right != null; } }          // 왼족, 오른족 둘다 있을때
        }
    }
}
