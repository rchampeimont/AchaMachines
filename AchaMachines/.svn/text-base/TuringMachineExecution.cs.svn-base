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
	public class TuringMachineExecution<TAlphabet,TState>
	{
		
		

		private TuringMachineExecutionById executionById;
		
		/* Reference to the definition. */
		private TuringMachineDefinition<TAlphabet,TState> definition;
		public TuringMachineDefinition<TAlphabet,TState> Definition {
			get {
				return definition;
			}
		}
		
		
		
		
		// Current state. You can change it while the machine is running
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
		
		public bool Halted {
			get {
				return executionById.Halted;
			}
		}
		
		// Tape is available as read/write. (so be carefull if you write)
		public LinkedList<int> Tape {
			get {
				return executionById.Tape;
			}
			set {
				executionById.Tape = value;
			}
		}
		
		// Cursor is available as read/write (so be carefull if you write)
		public LinkedListNode<int> Cursor {
			get {
				return executionById.Cursor;
			}
			set {
				executionById.Cursor = value;
			}
		}
		
		
		// You are expected to build objects of this class using the NewExecution
		// methods of TuringMachineDefinition.
		internal TuringMachineExecution(TuringMachineDefinition<TAlphabet,TState> machineDefinition)
		{
			definition = machineDefinition;
			executionById = new TuringMachineExecutionById(machineDefinition.definitionById);
		}
		
			
		

		
		
		/*
		// Make a copy of the machine, ie. we keep the same definition, and start
		// with this object state as the state of the new object.
		public FiniteStateMachineExecution<TAlphabet,TState> Clone() {
			var newExecution = new FiniteStateMachineExecution<TAlphabet,TState>();
			newExecution.definition = this.definition;
			newExecution.executionById = this.executionById.Clone();
			return newExecution;
		}
		*/
		
		// Do one step: change state, write letter, move.
		public void DoOneStep() {
			executionById.DoOneStep();
		}
		
		// Do n steps. This is equivalent to invoking DoOneStep() n times.
		public void DoManySteps(int n) {
			executionById.DoManySteps(n);
		}
			
		// Work until the machine stops. Note that some Turing machines don't halt.
		// Returns -2 if halted in accept state, -1 if halted in reject state.
		public int WorkUntilHalts() {
			return executionById.WorkUntilHalts();
		}
		
		public string FullStateIdsAsString() {
			return executionById.FullStateAsString();
		}
		
		public string TapeIdsAsString() {
			return executionById.TapeAsString();
		}
		

	}
}
