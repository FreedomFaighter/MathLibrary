from sympy.solvers import solve
from sympy import x, y, Symbol
from math import exp
from sympy import DiracDelta, oo

def secondOrderSolutionFrom(a, b, c):
	x = Symbol('x')
	if(a!=0):
		roots = solve(a*x**2+b*x+c)
		c1 = Symbol('c1')
		c2 = Symbol('c2')
		return c1*exp(roots[1]*x)+c2*exp(roots[2]*x)
	else:
		return (c1+c2)*exp(oo*x)