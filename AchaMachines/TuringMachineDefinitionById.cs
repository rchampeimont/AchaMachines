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
using System.Collections.Generic;

// This file contains only internal code.

namespace AchaMachines
{
	internal class TuringMachineDefinitionById
	{
		/* Tape alphabet. */
		private int tapeAlphabetSize;
		public int TapeAlphabetSize {
			get {
				return tapeAlphabetSize;
			}
		}
		
		
		
		/* Input alphabet. */
		private FrozenVector<bool> inputAlphabet;
		public bool InputAlphabet(int l) {
			return inputAlphabet[l];
		}
		
		
	
		/* State. */
		private int numberOfStates;
		public int NumberOfStates {
			get {
				return numberOfStates;
			}
		}
		
		
		
		/* Initial. */
		private int initialState;
		public int InitialState {
			get {
				return initialState;
			}
		}
		
		
		
		/* Blank symbol. */
		private int blank;
		public int Blank {
			get {
				return blank;
			}
		}
		
		
		
		/* Transition matrix. */
		private FrozenMatrix<int> transitionMatrix; // [state, letter] -> state
		// Returns transitionMatrix[state, j].
		// See main constructor for how this matrix is defined.
		public int TransitionMatrix(int state, int j) {
			return transitionMatrix[state, j];
		}
		

		
		
		// This constructor creates a Turing machine definition. The arguments are:
		// (Note: We use "symbol" and "letter" as synonims.)
		// - the tape alphabet size (int, >= 1)
		// - a blank symbol (>= 0, <(alphabet size))
		// - the set of letters that are part of the input alphabet, subset of the tape alphabet,
		//   it is an array of bools of length = alphabet size. The blank symbol
		//   must not be part of the input alphabet. newMachineInputAlphabet[a] contains
		//   true of false whether or not letter a is part of the input alphabet.
		// - the number of states (int, >= 1)
		// - an initial state (>= 0, <(number of states))
		// - a transition matrix, with lines being states, 3 columns beeing one letter and content being
		//   the new state, the new letter and the direction.
		//   ie. newMachinesTransitions[s1,3*a] = s2 means that when the machine is in state s1
		//   and reads letter a, it goes in state s2. Special values -1 and -2 respectively mean
		//   reject and accept.
		//   newMachinesTransitions[s1,3*a+1] = a2 means the written letter is a2
		//   newMachinesTransitions[s1,3*a+2] = d means the machine will go in direction d
		//   with 0 beeing "don't move", 1 beeing "go one step right", and 2 beeing "go one step left". 
		public TuringMachineDefinitionById(int newMachineTapeAlphabetSize,
		                                   int newMachineBlankSymbol,
		                                   FrozenVector<bool> newMachineInputAlphabet,
		                                   int newMachineNumberOfStates,
		                                   int newMachineInitialState,
		                                   FrozenMatrix<int> newMachineTransitionMatrix) {
			
			/* Store tape alphabet size. */
			if (!(newMachineTapeAlphabetSize >= 1)) {
				throw new TuringMachineException("Tape alphabet size should be >= 1, but it is " + newMachineTapeAlphabetSize);
			}
			tapeAlphabetSize = newMachineTapeAlphabetSize;
			
			
			
			/* Now check and store the blank symbol.. */
			if (!(newMachineBlankSymbol >= 0 && newMachineBlankSymbol < tapeAlphabetSize)) {
				throw new TuringMachineException("Blank symbol ("
				                                 + newMachineBlankSymbol
				                                 + ") is out of range. It should be in [0,"
				                                 + tapeAlphabetSize
				                                 +"[");
			}
			blank = newMachineBlankSymbol;
			
			
			
			/* Chack and store input alphabet. */
			if (newMachineInputAlphabet != null) {
				if (newMachineInputAlphabet.Length != newMachineTapeAlphabetSize) {
					throw new TuringMachineException("Given input alphabet vector length ("
					                                 + newMachineInputAlphabet.Length
					                                 + ") should equal tape alphabet size ("
					                                 + tapeAlphabetSize
					                                 + ").");
				}
				if (newMachineInputAlphabet[newMachineBlankSymbol]) {
					throw new TuringMachineException("Blank symbol must not be part of input alphabet.");
				}
				inputAlphabet = newMachineInputAlphabet;
			} else {
				// All but the blank symbol.
				var inputAlphabetTempMatrix = new bool[tapeAlphabetSize];
				for (int i=0; i<tapeAlphabetSize; i++) {
					if (i != newMachineBlankSymbol) {
						inputAlphabetTempMatrix[i] = true;
					} else {
						inputAlphabetTempMatrix[i] = false;
					}
				}
				inputAlphabet = new FrozenVector<bool>(inputAlphabetTempMatrix);
			}
			
			
			
			/* Store number of states. */
			if (!(newMachineNumberOfStates >= 1)) {
				throw new TuringMachineException("Number of states should be >= 1, but it is " + tapeAlphabetSize);
			}
			numberOfStates = newMachineNumberOfStates;
			
			
			
			/* Now check and store the initial state. */
			if (!(newMachineInitialState >= 0 && newMachineInitialState < numberOfStates)) {
				throw new TuringMachineException("Initial state ("
				                                 + newMachineInitialState
				                                 + ") is out of range. It should be in [0,"
				                                 + numberOfStates
				                                 +"[");
			}
			initialState = newMachineInitialState;
			

			
			/* Now check the transition matrix and store optimized version
			 * of this matrix using ints for destination states. */
			if (newMachineTransitionMatrix.GetLength(0) != numberOfStates) {
				throw new TuringMachineException("Transition matrix has "
				                                      + newMachineTransitionMatrix.GetLength(0)
				                                      + " lines while it should have "
				                                      + numberOfStates
				                                      + " lines which is the number of states."); 
			}
			if (newMachineTransitionMatrix.GetLength(1) != 3*tapeAlphabetSize) {
				throw new TuringMachineException("Transition matrix has "
				                                      + newMachineTransitionMatrix.GetLength(1)
				                                      + " columns while it should have "
				                                      + (3 * tapeAlphabetSize)
				                                      + " columns which is 3 times the number of letters in the alphabet."); 
			}
			for (int s=0; s<numberOfStates; s++) {
				for (int l=0; l<tapeAlphabetSize; l+=3) {
					if (!(newMachineTransitionMatrix[s,l] >= -2 && newMachineTransitionMatrix[s,l] < numberOfStates)) {
						throw new TuringMachineException("Transitions matrix references a state ("
						                                      + newMachineTransitionMatrix[s,l]
						                                      + ") that is out of range. It should be in [-2,"
						                                      + numberOfStates
						                                      +"[");
					}
					if (!(newMachineTransitionMatrix[s,l+1] >= 0 && newMachineTransitionMatrix[s,l+1] < tapeAlphabetSize)) {
						throw new TuringMachineException("Transitions matrix references a letter ("
						                                      + newMachineTransitionMatrix[s,l]
						                                      + ") that is out of range. It should be in [0,"
						                                      + tapeAlphabetSize
						                                      +"[");
					}
					if (!(newMachineTransitionMatrix[s,l+2] >= -1 && newMachineTransitionMatrix[s,l+2] <= 1)) {
						throw new TuringMachineException("Transitions matrix references a direction ("
						                                      + newMachineTransitionMatrix[s,l]
						                                      + ") that is out of range. It should be in [-1,1].");
					}
				}
			}
			transitionMatrix = newMachineTransitionMatrix;
		}
		
		
		
			                                
	}
}
