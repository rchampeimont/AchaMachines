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

// This contains only internal code.

namespace AchaMachines
{
	internal class FiniteStateMachineExecutionById
	{
		/* Reference to the definition. */
		private FiniteStateMachineDefinitionById definition;
		public FiniteStateMachineDefinitionById Definition {
			get {
				return definition;
			}
		}
		
		
		
		/* Current state. */
		internal int state;
		public int State {
			get {
				return state;
			}
			set {
				state = value;
			}
		}
		
		
		
		/* This constructor creates a finite state machine execution from a finite state machine definition.
		 * The created machine starts at the initial state (defined in the definition).
		 */
		public FiniteStateMachineExecutionById(FiniteStateMachineDefinitionById machineDefinition)
		{
			definition = machineDefinition;
			state = definition.InitialState;
		}
		
		private FiniteStateMachineExecutionById()
		{
		}
		
		
		
		/* Reads a letter or a word, and change state accordingly. */
		public void Read(int letter) {
			state = definition.Transition(state, letter);
		}
		public void Read(IEnumerable<int> word) {
			foreach (int letter in word) {
				Read(letter);
			}
		}
		
		
		
		/* Tells if the machine is in a final state. */
		public bool InFinalState {
			get {
				return definition.IsFinal(state);
			}
		}
		
		// Clone execution, keeping a reference to the same definition.
		public FiniteStateMachineExecutionById Clone() {
			var newExecutionById = new FiniteStateMachineExecutionById();
			newExecutionById.definition = this.definition;
			newExecutionById.state = this.state;
			return newExecutionById;
		}
	}
}
