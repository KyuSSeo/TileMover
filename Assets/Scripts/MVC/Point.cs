using UnityEngine;


// 2차원 좌표를 표현하기 위한 클래스
[System.Serializable]
public class Point
{

    public int x;
    public int y;
    /// <summary>
    /// x, y 좌표 값을 받아 Point 객체를 초기화합니다.
    /// </summary>  
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }


    /// <summary>
    /// 타일 a와 타일b의 좌표를 비교하여 같으면 True를 반환
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator ==(Point a, Point b)
    {
        return a.x == b.x && a.y == b.y;
    }

    /// <summary>
    /// 타일 a와 타일b의 좌표를 비교하여 다르면 True를 반환
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }

    /// <summary>
    /// 두 타일의 좌표를 더한 좌표를 반환
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }

    /// <summary>
    /// 두 타일의 좌표를 뺀 새로운 좌표를 반환
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static Point operator -(Point p1, Point p2)
    {
        return new Point(p1.x - p2.x, p1.y - p2.y);
    }



    /// <summary>
    /// 객체가 타입이 Point 타입이고 좌표가 같으면 true.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (obj is Point)
        {
            Point p = (Point)obj;
            return x == p.x && y == p.y;
        }
        return false;
    }
    /// <summary>
    /// Point 타입끼리 비교 후 좌표가 같으면 true.
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public bool Equals(Point p)
    {
        return x == p.x && y == p.y;
    }

    /// <summary>
    /// x와 y 값을 XOR 연산하여 해시 코드를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return x ^ y;
    }
    /// <summary>
    /// 좌표를 문자열 반환
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }
}
