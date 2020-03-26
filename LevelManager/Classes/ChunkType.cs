namespace Classes
{
    public enum ChunkType
    {
        //no path
        Full = 0,
        //dead ends - 1 way paths
        LeftDeadEnd = 1,
        RightDeadEnd = 2,
        UpDeadEnd = 3,
        DownDeadEnd = 4,
        //straight paths - 2 way paths
        Horizontal = 5,
        Vertical = 6,
        //corners - 2 way paths
        UpRightCorner = 7,
        DownRightCorner = 8,
        DownLeftCorner = 9,
        UpLeftCorner = 10,
        //T-shape paths - 3 way paths
        HorizontalUp = 11,
        HorizontalDown = 12,
        VerticalLeft = 13,
        VerticalRight = 14,
        //crossroad - 4 way paths
        Crossroad = 15
    }
}
