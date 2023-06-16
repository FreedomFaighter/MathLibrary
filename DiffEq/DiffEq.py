from code import interact
from numbers import Integral
from sympy.solvers import solve
from sympy.abc import x, y, c, Symbol
from sympy import oo, integrate, exp, Derivative
"""
Nagle, R. Kent, Saff, E. B., Snider, Arthur David, fundamentals of Differential Equations, Pearson Education, Inc., 2008, ISBN 0-321-38841-0
"""

def secondOrderSolutionFrom(a, b, c):
	c1 = Symbol('c1')
	c2 = Symbol('c2')
	if(a==0):
		raise Error('not second ordered differential equation')
	else:
		roots = solve(a*x**2+b*x+c)
	c1*exp(roots[1]*x)+c2*exp(roots[2]*x)

"""
Seperable Equations of the form dy/dx=f(x,y)
page 40-41
"""
def seperableEquation(Fg, Fp):
	return integrate(1 / Fp, y) - integrate(Fg, x) - Symbol('c')
"""

"""
def linearFirstOrderEquation(Fa1, Fa0, Fb):
	if Fa0 is None:
		integrate(Fb / Fa1) + c
	else if Derivative(Fa1) is Fa0:
		integrate(Fb, x) / Fa1
	else if Fa1 is 0:
		raise('Error a_{1}(x) is 0')

def standardForm(Fp, Fq):
	Fmu = exp(integrate(Fp, x))
	integrate(Fmu*Fq+c, x) / Fmu

def exactEquation(Fmxy, Fnxy):
	integrate(Fmxy,x) - integrate(Fnxy-Derivative(integrate(Fmxy,x),y))
"""
page 76
n = {x|x ∈ I\{0,1}} : standardForm(Fp, Fq)-y**(n-1)
n = {0,1} : standardForm(Fp, Fq)
"""
def bernoulliEquation(Fp, Fq, n, y):
	ans=standardForm(Fp, Fq)
	if n in range([0,1]):
		ans
	else:
		ans-y**(n-1)

