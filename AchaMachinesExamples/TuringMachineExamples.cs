// Copyright (c) 2009 River Champeimont
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using AchaMachines;
using AchaMachinesExamples;

namespace AchaMachinesExamples
{
	public class TuringMachineExamples
	{
		public static void ThreeStateBusyBeaver() {
			// See http://en.wikipedia.org/wiki/Turing_machine_examples#3-state_Busy_Beaver
			Console.WriteLine("BEGIN: ThreeStateBusyBeaver");
			
			bool[] inputAlphabet = { false, true };
			int[,] transitions = {
				// read 0        read 1
				{  1,  1,  1,     2,  1, -1  }, // When in state 0
				{  0,  1, -1,     1,  1,  1  }, // When in state 1
				{  1,  1, -1,    -2,  1,  0  }, // When in state 2
			};
			var beaverDef = new TuringMachineDefinition<int,int>(null,
			                                                     0,
			                                                     new FrozenVector<bool>(inputAlphabet),
			                                                     null,
			                                                     0,
			                                                     new FrozenMatrix<int>(transitions));
			var beaverExec = beaverDef.NewExecution();
			int i = 0;
			while (!beaverExec.Halted) {
				i++;
				beaverExec.DoOneStep();
				Console.WriteLine(beaverExec.FullStateIdsAsString());
			}
			Console.WriteLine("Busy beaver took " + i + " steps.");
			
			Console.WriteLine("END: ThreeStateBusyBeaver");
		}
		
		public static TuringMachineDefinition<int,int> OddNumberWrittenInBinaryDefinition() {
			return FiniteStateMachineExamples.OddNumberWrittenInBinaryDefinition().ToTuringMachineDefinitionById();
		}
		
		private static string StringOfIntArray(int[] a) {
			string s = "";
			foreach (int x in a) {
				s += x;
			}
			return s;
		}
		
		public static void OddNumberWrittenInBinary() {
			Console.WriteLine("BEGIN: OddNumberWrittenInBinary (TM version)");
			
			var tmDefinition = OddNumberWrittenInBinaryDefinition();
			int[] x1 = {};
			int[] x2 = { 0 };
			int[] x3 = { 1 };
			int[] x4 = { 0, 0, 0, 1, 0, 0, 1 };
			int[] x5 = { 1, 0, 0, 0 };
			Console.WriteLine("Let us test some binary number and look if they are odd:");
			int[][] numbers = { x1, x2, x3, x4, x5 };
			foreach (int[] number in numbers) {
				Console.WriteLine("\"" + StringOfIntArray(number) + "\" is " + (tmDefinition.AcceptsById(number) ? "odd" : "even") + ".");
			}
			
			Console.WriteLine("END: OddNumberWrittenInBinary (TM version)");
		}
		
		public static void AllTests() {
			ThreeStateBusyBeaver();
			OddNumberWrittenInBinary();
		}
	}
}
