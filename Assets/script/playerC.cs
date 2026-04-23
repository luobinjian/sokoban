using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class playerC : MonoBehaviour
{
    Vector2 moveDir;

    int iceCount = 0;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir = Vector2.right;
            move(moveDir);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir = Vector2.left;
            move(moveDir);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDir = Vector2.up;
            move(moveDir);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDir = Vector2.down;
            move(moveDir);
        }
    }

    private bool canMove(Vector2 dir)
    {
        iceCount = 0;
        // 使用Raycast检测前方是否有墙壁
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3) dir, dir, 0.5f);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                print("撞墙");
                return false;
            }
            else if (hit.collider.CompareTag("Box"))
            {
                // 检查箱子是否可以移动
                RaycastHit2D boxHit = Physics2D.Raycast(transform.position + (Vector3) dir * 2 , dir, 0.5f);
                if (boxHit.collider != null && (boxHit.collider.CompareTag("Wall") || boxHit.collider.CompareTag("Box")))
                {
                    return false;
                }
            }
            else if (hit.collider.CompareTag("ice"))
            {
                for(int i = 1; i<20 ; i++)
                {
                    RaycastHit2D iceHit = Physics2D.Raycast(transform.position + (Vector3) dir * i, dir, 0.5f);
                    if (iceHit.collider != null && iceHit.collider.CompareTag("ice"))
                    {
                        iceCount++;
                        Debug.Log(iceCount);
                    }
                    else if(iceHit.collider != null && (iceHit.collider.CompareTag("Wall") || iceHit.collider.CompareTag("Box")))
                    {
                        iceCount--;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                print("冰冰" + iceCount);
            }
        }
        
        return true;
    }

    private void move(Vector2 dir)
    {
        if (canMove(dir))
        {
            Vector2 newPos = (Vector2)transform.position + dir;
            if(iceCount > 0)
            {
                newPos = (Vector2)transform.position + dir * (iceCount + 1);
            }
            else
            {
                newPos = (Vector2)transform.position + dir;
            }
            
            // 检查是否有箱子
            Collider2D box = Physics2D.OverlapPoint(newPos);
            if (box != null && box.CompareTag("Box"))
            {
                box.transform.position = (Vector2)box.transform.position + dir;
            }
            
            transform.position = newPos;
        }
    }
}