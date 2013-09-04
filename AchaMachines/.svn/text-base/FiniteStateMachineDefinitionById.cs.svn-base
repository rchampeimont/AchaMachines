// Copyright (c) 2009 Raphael Champeimont
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
	internal class FiniteStateMachineDefinitionById
	{
		/* Alphabet. */
		private int alphabetSize;
		public int AlphabetSize {
			get {
				return alphabetSize;
			}
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
		
		
		
		/* Transition matrix. */
		private FrozenMatrix<int> transitionMatrix; // [state, letter] -> state
		// Given a state s (int) and a letter l (int), computes the new state in which the
		// machine would go if it read the letter l while in state s.
		// The new state (int) is returned.
		public int Transition(int state, int letter) {
			return transitionMatrix[state, letter];
		}
		
		
		
		/* Final states. */
		private FrozenVector<bool> finalStates;
		/* Tells if the given state is final or not. */
		public bool IsFinal(int state) {
			try {
				return finalStates[state];
			} catch (IndexOutOfRangeException) {
				throw new FiniteStateMachineException("IsFinal: State "
				                                      + state
				                                      + " is out of range [0,"
				                                      + numberOfStates
				                                      + "[.");
			}
		}

		
		
		
		/* This constructor creates a finite state machine definition. The arguments are:
		 * - the alphabet size (int, >= 0)
		 * - the number of states (int, >= 1)
		 * - an initial state (>= 0, <(number of states))
		 * - a transition matrix, with lines being states, columns beeing letters and content being the new states,
		 *   ie newMachinesTransitions[s1,a] = s2 means that when the machine is in state s1
		 *   and reads letter a, it goes in state s2.
		 * - an array of booleans, as many as states, which say if the state is final or not
		 */
		public FiniteStateMachineDefinitionById(int newMachineAlphabetSize,
		                                        int newMachineNumberOfStates,
		                                        int newMachineInitialState,
		                                        FrozenMatrix<int> newMachineTransitionMatrix,
		                                        FrozenVector<bool> newMachineFinalStates)
		{
			
			/* Store alphabet size. */
			if (!(newMachineAlphabetSize >= 0)) {
				throw new FiniteStateMachineException("Alphabet size is negative (" + newMachineAlphabetSize + ")");
			}
			alphabetSize = newMachineAlphabetSize;
			
			/* Store number of states. */
			if (!(newMachineNumberOfStates >= 1)) {
				throw new FiniteStateMachineException("Number of states should be >= 1, but it is " + newMachineAlphabetSize);
			}
			numberOfStates = newMachineNumberOfStates;
			
			/* Now check and store the initial state. */
			if (!(newMachineInitialState >= 0 && newMachineInitialState < numberOfStates)) {
				throw new FiniteStateMachineException("Initial state ("
				                                      + newMachineInitialState
				                                      + ") is out of range. It should be in [0,"
				                                      + numberOfStates
				                                      +"[");
			}
			initialState = newMachineInitialState;
			
			/* Now check the transition matrix and store optimized version
			 * of this matrix using ints for destination states. */
			if (newMachineTransitionMatrix.GetLength(0) != numberOfStates) {
				throw new FiniteStateMachineException("Transition matrix has "
				                                      + newMachineTransitionMatrix.GetLength(0)
				                                      + " lines while it should have "
				                                      + numberOfStates
				                                      + " lines which is the number of states."); 
			}
			if (newMachineTransitionMatrix.GetLength(1) != alphabetSize) {
				throw new FiniteStateMachineException("Transition matrix has "
				                                      + newMachineTransitionMatrix.GetLength(1)
				                                      + " columns while it should have "
				                                      + alphabetSize
				                                      + " columns which is the number of letters in the alphabet."); 
			}
			for (int s=0; s<numberOfStates; s++) {
				for (int l=0; l<alphabetSize; l++) {
					if (!(newMachineTransitionMatrix[s,l] >= 0 && newMachineTransitionMatrix[s,l] < numberOfStates)) {
						throw new FiniteStateMachineException("Transitions matrix references a state ("
						                                      + newMachineTransitionMatrix[s,l]
						                                      + ") that is out of range. It should be in [0,"
						                                      + numberOfStates
						                                      +"[");
					}
				}
			}
			transitionMatrix = newMachineTransitionMatrix;
			
			/* Check and store final states. */
			if (newMachineFinalStates.Length != numberOfStates) {
				throw new FiniteStateMachineException("Final states array has "
				                                      + newMachineFinalStates.Length
				                                      + " elements while it should have "
				                                      + numberOfStates
				                                      + " elements which is the number of states.");
			}
			finalStates = newMachineFinalStates;
		}
		
		
		
			                                
	}
}
