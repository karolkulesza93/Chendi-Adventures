using System.Collections.Generic;
using System.Linq;


namespace Classes
{
    public static class Chunk
    {
        static List<List<char>> GetChunk(ChunkType type)
        {
            List<List<char>> chunk = new List<List<char>>();
            chunk.Clear();
            switch (type)
            {
                case ChunkType.Full:
                    {
                        for (int i = 0; i < 8; i++) chunk.Append(new List<char> { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        break;
                    }
                case ChunkType.Horizontal:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char> { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char> { '.', '.', '.', '.', '.', '.', '.', '.' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char> { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        break;
                    }
                case ChunkType.Vertical:
                    {
                        for (int i = 0; i < 8; i++) chunk.Append(new List<char> { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        break;
                    }
                case ChunkType.LeftDeadEnd:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char> { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char> { 'D', 'D', '.', '.', '.', '.', '.', '.' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char> { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        break;
                    }
                case ChunkType.UpDeadEnd:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        for (int i = 0; i < 6; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        break;
                    }
                case ChunkType.RightDeadEnd:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char> { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char> { '.', '.', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char> { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        break;
                    }
                case ChunkType.DownDeadEnd:
                    {
                        for (int i = 0; i < 6; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        break;
                    }
                case ChunkType.UpRightCorner:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', '.', '.' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        break;
                    }
                case ChunkType.DownRightCorner:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', '.', '.' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        break;
                    }
                case ChunkType.DownLeftCorner:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char>() { '.', '.', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        break;
                    }
                case ChunkType.UpLeftCorner:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char>() { '.', '.', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        break;
                    }
                case ChunkType.HorizontalUp:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char>() { '.', '.', '.', '.', '.', '.', '.', '.' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        break;
                    }
                case ChunkType.HorizontalDown:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', 'D', 'D', 'D', 'D', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char>() { '.', '.', '.', '.', '.', '.', '.', '.' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        break;
                    }
                case ChunkType.VerticalLeft:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char>() { '.', '.', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        break;
                    }
                case ChunkType.VerticalRight:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', '.', '.' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        break;
                    }
                case ChunkType.Crossroad:
                    {
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        for (int i = 0; i < 4; i++) chunk.Append(new List<char>() { '.', '.', '.', '.', '.', '.', '.', '.' });
                        for (int i = 0; i < 2; i++) chunk.Append(new List<char>() { 'D', 'D', '.', '.', '.', '.', 'D', 'D' });
                        break;
                    }
            }
            return chunk;
        }
    }
}
