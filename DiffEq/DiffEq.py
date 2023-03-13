from aifc import Error
from sympy.solvers import solve
from sympy import x, y, Symbol
from sympy import oo, integrate,exp

def secondOrderSolutionFrom(a, b, c):
	x = Symbol('x')
	c1 = Symbol('c1')
	c2 = Symbol('c2')
	if(a!=0):
		roots = solve(a*x**2+b*x+c)
		return c1*exp(roots[1]*x)+c2*exp(roots[2]*x)
	elif b!=0:
		return ((c1+c2)*exp(oo*x),-(c/b)*x)
#	else:
#		return Error("Not a differential equation of first or second order")

def seperableEquations(Fg, Fp):
	x = Symbol('x')
	y = Symbol('y')
	c = Symbol('c')
	h = integrate(1 / Fp, y)
	g = integrate(Fg, x)
#	return h - g - c
