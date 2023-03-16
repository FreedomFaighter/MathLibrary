from code import interact
from numbers import Integral
from sympy.solvers import solve
from sympy.abc import x, y, c, Symbol
from sympy import oo, integrate, exp, Derivative

def secondOrderSolutionFrom(a, b, c):
	c1 = Symbol('c1')
	c2 = Symbol('c2')
	if(a==0):
	  raise Error('not second ordered differential equation')
	else:
	  roots = solve(a*x**2+b*x+c)
	  return c1*exp(roots[1]*x)+c2*exp(roots[2]*x)

def seperableEquations(Fg, Fp):
	return integrate(1 / Fp, y) - integrate(Fg, x) - Symbol('c')
	
def linearFirstOrderEquation(Fa1, Fa0, Fb):
	if Fa0 is None:
		return integrate(Fb / Fa1) + c
	else:
		if Derivative(Fa1) is Fa0:
			return integrate(Fb, x) / Fa1

def standardForm(Fp, Fq):
	Fmu = exp(integrate(Fp, x))
	return integrate(Fmu*Fq+c, x) / Fmu

def exactEquations(Fmxy, Fnxy):
	return integrate(Fmxy,x) - integrate(Fnxy-Derivative(integrate(Fmxy,x),y))
