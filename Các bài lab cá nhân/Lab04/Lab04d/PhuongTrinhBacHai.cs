namespace Lab04d;

public class PhuongTrinhBacHai
{
    public double A { get; set; }

    public double B { get; set; }

    public double C { get; set; }

    public PhuongTrinhBacHai(
        double a,
        double b,
        double c = 0)
    {
        A = a;
        B = b;
        C = c;
    }

    public string GiaiBacNhat()
    {
        if (A == 0)
        {
            if (B == 0)
                return "Phương trình vô số nghiệm.";

            return "Phương trình vô nghiệm.";
        }

        double x = -B / A;

        return $"Phương trình có nghiệm x = {x:0.##}";
    }

    public string GiaiBacHai()
    {
        if (A == 0)
        {
            PhuongTrinhBacHai pt =
                new PhuongTrinhBacHai(B, C);

            return pt.GiaiBacNhat();
        }

        double delta =
            B * B - 4 * A * C;

        if (delta < 0)
            return "Phương trình vô nghiệm.";

        if (delta == 0)
        {
            double x = -B / (2 * A);

            return
                $"Phương trình có nghiệm kép x = {x:0.##}";
        }

        double x1 =
            (-B + Math.Sqrt(delta))
            / (2 * A);

        double x2 =
            (-B - Math.Sqrt(delta))
            / (2 * A);

        return
            $"x1 = {x1:0.##}; x2 = {x2:0.##}";
    }
}