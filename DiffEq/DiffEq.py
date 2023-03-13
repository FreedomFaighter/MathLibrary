from sympy.solvers import solve
from sympy import x, y, Symbol
from math import exp

def secondOrderSolutionFrom(a, b, c):
	if(a!=0):
		x = Symbol('x')
		roots = solve(a*x**2+b*x+c)
		c1 = Symbol('c1')
		c2 = Symbol('c2')
		return c1*exp(roots[1]*x)+c2*exp(roots[2]*x)