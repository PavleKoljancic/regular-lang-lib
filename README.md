# Regular Language Library

This library provides functionality for working with regular languages represented by Deterministic Finite Automata (DFA), epsilon-Non-deterministic Finite Automata (ε-NFA), and regular expressions. It includes various operations and transformations for these representations, as well as tools for analyzing and minimizing automata.

## Features

### Finite Automata Execution
- **DFA Execution:** Execute deterministic finite automata provided by the user.
- **ε-NFA Execution:** Execute epsilon-non-deterministic finite automata provided by the user.

### Language Operations
- **Union:** Construct the union of two regular languages.
- **Intersection:** Construct the intersection of two regular languages.
- **Difference:** Construct the difference between two regular languages.
- **Concatenation:** Concatenate two regular languages.
- **Complement:** Construct the complement of a regular language.
- **Kleene Star:** Apply the Kleene star operation to a regular language.
- **Chaining Operations:** Support chaining of the above operations.

### Language Properties
- **Shortest and Longest Word Length:** Determine the lengths of the shortest and longest words in the language.
- **Language Finiteness:** Check if the language is finite.

### Automata Minimization
- **State Minimization:** Minimize the number of states in a deterministic finite automaton.

### Transformations
- **ε-NFA to DFA:** Transform an epsilon-non-deterministic finite automaton into a deterministic finite automaton.
- **Regular Expression to Automaton:** Transform a regular expression into a finite automaton.

### Regular Language Comparison
- **Equality Check:** Compare two regular language representations (including regular expressions) for language equality.

## Applications

### Regular Language Specification and Membership Testing
- **Specification Loading:** Load a regular language representation from a specification (supports DFA, ε-NFA, and regular expressions).
- **Membership Testing:** Test if specified strings belong to the represented language.
- **Lexical Analysis:** Perform lexical analysis on the specification to detect and report lexical errors, including the number of lines with errors.

### State Machine Code Generation
- **State Machine Simulation Code Generation:** Generate code for simulating a state machine based on a DFA specification.
  - **Entry and Exit Reactions:** Allow users to specify reactions to entry and exit events for each state.
  - **Transition Reactions:** Allow users to specify reactions to transitions for each alphabet symbol, depending on the current state.
  - **Chaining Reactions:** Enable chaining of reactions to effectively form a chain of reactions for a given event.
