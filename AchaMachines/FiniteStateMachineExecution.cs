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

// IMPLEMENTATION STATUS: STABLE (you can assume the interface will not change,
// or will change but keep backward compatibility).

namespace AchaMachines
{
	/*
	This class implements a running finite state machine, ie an object
	of this classs contains a finite state machine definition
	(see class FiniteStateMachineDefinition),
	as well	as its current state. In fact, an object of this class
	has a reference to the original definition, only the current state
	is stored.
	*/
	public class FiniteStateMachineExecution<TAlphabet,TState>
	{
		private FiniteStateMachineExecutionById executionById;
		
		/* Reference to the definition. */
		private FiniteStateMachineDefinition<TAlphabet,TState> definition;
		public FiniteStateMachineDefinition<TAlphabet,TState> Definition {
			get {
				return definition;
			}
		}
		
		
		
		
		// Current state. You can change it if you want while the machine is running.
		// (so be carefull if you write)
		public int StateId {
			get {
				return executionById.State;
			}
			set {
				executionById.State = value;
			}
		}
		public TState State {
			get {
				return definition.StateOfStateId(StateId);
			}
			set {
				StateId = definition.StateIdOfState(value);
			}
		}
		
		
		
		/* This constructor creates a finite state machine execution from a finite state machine definition.
		 * The created machine starts at the initial state (defined in the definition).
		 */
		public FiniteStateMachineExecution(FiniteStateMachineDefinition<TAlphabet,TState> machineDefinition)
		{
			definition = machineDefinition;
			executionById = new FiniteStateMachineExecutionById(machineDefinition.definitionById);
		}
		
		private FiniteStateMachineExecution() {}
		
		
		
		/* Reads a letter or a word, and change state accordingly. */
		public void Read(TAlphabet letter) {
			int letterId;
			try {
				letterId = definition.LetterIdOfLetter(letter);
			} catch (KeyNotFoundException) {
				throw new FiniteStateMachineException("Invalid letter: \"" + letter + "\".");
			}
			ReadById(letterId);
		}
		public void Read(IEnumerable<TAlphabet> word) {
			foreach (TAlphabet letter in word) {
				Read(letter);
			}
		}
		// Same thing, but the word is given as a list of letter ids instead of letters (ie. TAlphabet type).
		public void ReadById(int letterId) {
			executionById.Read(letterId);
		}
		public void ReadById(IEnumerable<int> letterIds) {
			executionById.Read(letterIds);
		}
		
		
		
		/* Tells if the machine is in a final state. */
		public bool InFinalState {
			get {
				return executionById.InFinalState;
			}
		}
		
		// Make a copy of the machine, ie. we keep the same definition, and start
		// with this object state as the state of the new object.
		public FiniteStateMachineExecution<TAlphabet,TState> Clone() {
			var newExecution = new FiniteStateMachineExecution<TAlphabet,TState>();
			newExecution.definition = this.definition;
			newExecution.executionById = this.executionById.Clone();
			return newExecution;
		}
			
	}
}
