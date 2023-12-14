using System.Diagnostics;

namespace Parabolic_Reflector_Dish;

public class ParabolicDish
{
    private static int _width;
    private static int _height;

    public static void Main(string[] args)
    {
        var lines = File.ReadAllLines(@"C:\testutvikling\Parabolic Reflector Dish\Parabolic Reflector Dish\input.txt");
        _width = lines[0].Length;
        _height = lines.Length;

        // Part 1
        var stopwatch = Stopwatch.StartNew();
        var map = CreateMap(lines);
        long result = ComputeLoad(TiltNorth(map));

        Console.WriteLine("Result part 1 : " + result + " in " + stopwatch.ElapsedMilliseconds + "ms");
        stopwatch.Restart();

        // Part 2
        map = CreateMap(lines);

        var index = new Dictionary<string, long>();

        for (long i = 0; i < 1000000000; i++)
        {
            map = Cycle(map);
            string str = ToString(map);
            if (index.ContainsKey(str))
            {
                long delta = i - index[str];
                i += delta * ((1000000000 - i) / delta);
            }
            index[str] = i;
        }
        result = ComputeLoad(map);

        Console.WriteLine("Result part 2 : " + result + " in " + stopwatch.ElapsedMilliseconds + "ms");
    }

    private static long ComputeLoad(char[][] map)
    {
        long result = 0;
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (map[x][y] == 'O')
                {
                    result += _height - y;
                }
            }
        }
        return result;
    }

    public static string ToString(char[][] map)
    {
        var b = new System.Text.StringBuilder();
        foreach (var line in map)
        {
            b.Append(new string(line));
        }
        return b.ToString();
    }

    private static char[][] Rotate(char[][] map)
    {
        var result = new char[_height][];
        for (int x = 0; x < _width; x++)
        {
            result[x] = new char[_width];
            for (int y = 0; y < _height; y++)
            {
                result[x][y] = map[y][_height - x - 1];
            }
        }
        return result;
    }

    private static char[][] Cycle(char[][] map)
    {
        for (int i = 0; i < 4; i++) { map = Rotate(TiltNorth(map)); }
        return map;
    }

    private static char[][] CreateMap(string[] lines)
    {
        if (lines == null || lines.Length == 0)
            throw new ArgumentException("lines array is null or empty.");

        _height = lines.Length;
        _width = lines[0].Length;

        var map = new char[_height][];
        for (int y = 0; y < lines.Length; y++)
        {
            if (lines[y] == null || lines[y].Length != _width)
                throw new ArgumentException($"Line {y} is null or of incorrect length.");

            map[y] = new char[_width];
            for (int x = 0; x < lines[y].Length; x++)
            {
                map[y][x] = lines[y][x];
            }
        }
        return map;
    }


    private static char[][] TiltNorth(char[][] map)
    {
        for (int x = 0; x < _width; x++)
        {
            bool move = true;
            while (move)
            {
                move = false;
                for (int y = 1; y < _height; y++)
                {
                    if (map[x][y] == 'O' && map[x][y - 1] == '.')
                    {
                        map[x][y] = '.';
                        map[x][y - 1] = 'O';
                        move = true;
                    }
                }
            }
        }
        return map;
    }

    private static void Dump(char[][] map)
    {
        for (int y = 0; y < map[0].Length; y++)
        {
            for (int x = 0; x < map.Length; x++)
            {
                Console.Write(map[x][y]);
            }
            Console.WriteLine();
        }
    }
}