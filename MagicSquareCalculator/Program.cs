using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Numerics;
using BI = System.Numerics.BigInteger;

namespace MagicSquareCalculator {
	struct MagicSquare {
		static int Magics = 0;
		public BigInteger Start;
		public BigInteger A1, A2, A3, B1, B2, B3, C1, C2, C3;

		public void Iterate(BigInteger Start, BigInteger Len) {
			Len += Start;

			for (BigInteger a1 = Start; a1 < Len; a1++)
				for (BigInteger a2 = Start; a2 < Len; a2++)
					for (BigInteger a3 = Start; a3 < Len; a3++)

						for (BigInteger b1 = Start; b1 < Len; b1++)
							for (BigInteger b2 = Start; b2 < Len; b2++)
								for (BigInteger b3 = Start; b3 < Len; b3++)

									for (BigInteger c1 = Start; c1 < Len; c1++)
										for (BigInteger c2 = Start; c2 < Len; c2++)
											for (BigInteger c3 = Start; c3 < Len; c3++) {
												A1 = a1;
												A2 = a2;
												A3 = a3;
												B1 = b1;
												B2 = b2;
												B3 = b3;
												C1 = c1;
												C2 = c2;
												C3 = c3;

												if (IsMagic(a1, a2, a3, b1, b2, b3, c1, c2, c3)) {
													Program.Save(string.Format("Magic{0}.txt", Magics++));
													Console.WriteLine("Found magic square!");
													Console.WriteLine(this);
												}
											}
		}

		bool IsMagic(BI A1, BI A2, BI A3, BI B1, BI B2, BI B3, BI C1, BI C2, BI C3, int Exp = 2) {
			if (Compare(A1, A2, A3, B1, B2, B3, C1, C2, C3))
				return false;

			/*A1 = BigInteger.Pow(A1, Exp);
			A2 = BigInteger.Pow(A2, Exp);
			A3 = BigInteger.Pow(A3, Exp);
			B1 = BigInteger.Pow(B1, Exp);
			B2 = BigInteger.Pow(B2, Exp);
			B3 = BigInteger.Pow(B3, Exp);
			C1 = BigInteger.Pow(C1, Exp);
			C2 = BigInteger.Pow(C2, Exp);
			C3 = BigInteger.Pow(C3, Exp);*/

			BigInteger Expected = A1 + A2 + A3;
			if ((B1 + B2 + B3) == Expected) {
				if ((C1 + C2 + C3) == Expected) {
					if ((A1 + B2 + C3) == Expected) {
						if ((C1 + B2 + A3) == Expected) {
							return true;
						}
					}
				}
			}

			return false;
		}

		bool Compare(params BI[] Nums) {
			for (int i = 0; i < Nums.Length - 1; i++)
				for (int j = i + 1; j < Nums.Length; j++)
					if (Nums[i] == Nums[j])
						return true;
			return false;
		}

		public void FromLines(string[] Lines) {
			A1 = BigInteger.Parse(Lines[0]);
			A2 = BigInteger.Parse(Lines[1]);
			A3 = BigInteger.Parse(Lines[2]);
			B1 = BigInteger.Parse(Lines[3]);
			B2 = BigInteger.Parse(Lines[4]);
			B3 = BigInteger.Parse(Lines[5]);
			C1 = BigInteger.Parse(Lines[6]);
			C2 = BigInteger.Parse(Lines[7]);
			C3 = BigInteger.Parse(Lines[8]);
			Start = BigInteger.Parse(Lines[9]);
		}

		public string[] ToLines() {
			return new string[] {
				A1.ToString(), A2.ToString(), A3.ToString(),
				B1.ToString(), B2.ToString(), B3.ToString(),
				C1.ToString(), C2.ToString(), C3.ToString(),
				Start.ToString()
			};
		}

		public override string ToString() {
			return string.Format("{0}, {1}, {2} - {3}, {4}, {5} - {6}, {7}, {8}", A1, A2, A3, B1, B2, B3, C1, C2, C3);
		}
	}

	unsafe class Program {
		const int Exp = 2;

		static MagicSquare Square;

		public static void Load(string SaveFile) {
			Square = new MagicSquare();
			if (File.Exists(SaveFile))
				Square.FromLines(File.ReadAllLines(SaveFile));
		}

		public static void Save(string SaveFile) {
			/*if (File.Exists(SaveFile))
				File.Delete(SaveFile);*/
			File.WriteAllLines(SaveFile, Square.ToLines());
		}

		static void Main(string[] args) {
			Console.Title = "Magic square calculator";
			Load("Save.txt");
			BigInteger Len = 1000000;

			Thread PrinterThread = new Thread(() => {
				while (true) {
					Thread.Sleep(10000);
					Console.WriteLine(Square);
				}
			});
			PrinterThread.IsBackground = true;
			PrinterThread.Start();

			while (true) {
				Console.WriteLine("{0} to {1}", Square.Start, Square.Start + Len);
				Square.Iterate(Square.Start, Len);
				Save("Save.txt");
				Square.Start += Len;
			}
		}
	}
}
