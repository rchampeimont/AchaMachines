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

namespace AchaMachinesExamples
{
	
	
	public class FiniteStateMachineExamples
	{
		
		public static FiniteStateMachineDefinition<char,string> OddNumberWrittenInBinaryDefinition() {
			string[] states = {"even", "odd"};
			int[,] transitions = {
				// We read:
				// 0       1 
				{0, 1}, // when string read before represented an even number
				{0, 1}  // when string read before represented an odd number
			};
			bool[] finalStates = {false, true}; // "odd" is a final state
			var fsmDefinition =
				new FiniteStateMachineDefinition<char,string>("01".ToCharArray(),
				                                              states,
				                                              0, // 0 is an even number
				                                              transitions,
				                                              finalStates);
			return fsmDefinition;
		}
		
		public static void OddNumberWrittenInBinary()
		{
			Console.WriteLine("BEGIN: OddNumberWrittenInBinary (FSM version)");
			var fsmDefinition = OddNumberWrittenInBinaryDefinition();
			
			
			Console.WriteLine("We create a FiniteStateMachineExecution.");
			var fsmExecution = fsmDefinition.NewExecution();
			Console.WriteLine("Current state is \"" + fsmExecution.State + "\".");
			Console.Write("Let us read a 0.");
			fsmExecution.Read('0'); // we can supply a letter
			Console.WriteLine(" We are now in state \"" + fsmExecution.State + "\".");
			Console.Write("Let us read a 1.");
			fsmExecution.Read("1".ToCharArray()); // we can also supply a word (even if it is a singleton)
			Console.WriteLine(" We are now in state \"" + fsmExecution.State + "\".");
			Console.WriteLine("Are we in a final state ? " + (fsmExecution.InFinalState ? "yes" : "no") + "!");
			
			Console.WriteLine("Now let us test some binary number and look if they are odd:");
			string[] numbers = {"", "0", "1", "0001001", "1000"};
			foreach (string number in numbers) {
				Console.WriteLine("\"" + number + "\" is " + fsmDefinition.ReadAndReturnState(number) + ".");
			}
			
			
			Console.WriteLine("END: OddNumberWrittenInBinary (FSM version)");
		}
		
		public static void TrivialAcceptAllFSM() {
			Console.WriteLine("BEGIN: TrivialAcceptAllFSM");
			bool[] finalStates = { true };
			int[,] transitions = { {0, 0} };
			var fsmDefinition = new FiniteStateMachineDefinition<char,char>("01".ToCharArray(),
			                                                                "F".ToCharArray(),
			                                                                0,
			                                                                transitions,
			                                                                finalStates);
			string word = "0100010001110";
			Console.WriteLine("Does it accept " + word + "? " + (fsmDefinition.Accepts(word) ? "yes" : "no") + ".");
			Console.WriteLine("END: TrivialAcceptAllFSM");
			                                                            
		}
		
		public static void SimplestFSM() {
			Console.WriteLine("BEGIN: SimplestFSM");
			int[,] transitions = {{}};
			bool[] finalStates = {true};
			int[] word = {};
			var fsmDefinition = new FiniteStateMachineDefinition<int,int>(null, null, 0, transitions, finalStates);
			Console.WriteLine("Does it accept the empty word? " + (fsmDefinition.Accepts(word) ? "yes" : "no") + ".");
			Console.WriteLine("END: SimplestFSM");
		}
		
		public static void FreezeTests() {
			Console.WriteLine("BEGIN: FreezeTests");
			int[,] transitions = {{}};
			bool[] finalStates = {true};
			int[] word = {};
			var fsmDefinition = new FiniteStateMachineDefinition<int,int>(null,
			                                                              null,
			                                                              0,
			                                                              transitions,
			                                                              finalStates);
			Console.WriteLine("1 Does it accept the empty word (it should) ? " + (fsmDefinition.Accepts(word) ? "yes" : "no") + ".");
			finalStates[0] = false;
			Console.WriteLine("2 Does it still accept the empty word (it should) ? " + (fsmDefinition.Accepts(word) ? "yes" : "no") + ".");
			var v = new FrozenVector<bool>(finalStates);
			var fsmDefinition2 = new FiniteStateMachineDefinition<int,int>(null,
			                                                               null,
			                                                               0,
			                                                               new FrozenMatrix<int>(transitions),
			                                                               v);
			Console.WriteLine("3 Does this other one accept the empty word (it should not) ? " + (fsmDefinition2.Accepts(word) ? "yes" : "no") + ".");
			Console.WriteLine("END: FreezeTests");
		}
		
		public static void CloneTests() {
			Console.WriteLine("BEGIN: CloneTests");
			Console.WriteLine("Let Σ=ab, we consider the machine that accepts a*b*.");
			char[] alphabet = "ab".ToCharArray();
			int[,] transitions = {
				//a  b
				{ 0, 1 }, // class of "", subset of L
				{ 2, 1 }, // class of "b", subset of L
				{ 2, 2 }, // class of "ab" = Σ* - L
			};
			bool[] finalStates = {true, true, false};
			var fsmDefinition = new FiniteStateMachineDefinition<char,int>(alphabet, null, 0, transitions, finalStates);
			Console.WriteLine("Try some words, are they accepted?");
			string[] words = {"", "a", "aaaaa", "aaabbbbbb", "abaa", "bbbb", "bbbba", "abababa"};
			foreach (string word in words) {
				Console.WriteLine("\"" + word + "\" -> " + (fsmDefinition.Accepts(word.ToCharArray()) ? "yes" : "no"));
			}
			var afterReadingB = fsmDefinition.NewExecution();
			afterReadingB.Read("b".ToCharArray());
			Console.WriteLine("We now build a machine and we make it read a \"b\".");
			var copy1 = afterReadingB.Clone();
			copy1.Read("aaaaa".ToCharArray());
			Console.WriteLine("We make a first copy of it and make the copy read \"aaaaa\". Is it in final state? "
			                  + (copy1.InFinalState ? "yes" : "no")
			                  + ".");
			var copy2 = afterReadingB.Clone();
			copy2.Read("bbb".ToCharArray());
			Console.WriteLine("We make a second copy and make the copy read \"bbb\". Is it in final state? "
			                  + (copy2.InFinalState ? "yes" : "no")
			                  + ".");
			Console.WriteLine("END: CloneTests");
		}
		
		public static void AllTests() {
			FiniteStateMachineExamples.OddNumberWrittenInBinary();
			FiniteStateMachineExamples.TrivialAcceptAllFSM();
			FiniteStateMachineExamples.SimplestFSM();
			FiniteStateMachineExamples.FreezeTests();
			FiniteStateMachineExamples.CloneTests();
		}
		
	}
}
 