[Motivation](https://github.com/FreedomFaighter/GeneticRiskOnOddsRatios)

# Open form of the logistic equation

$$P\left(D|G_{i}\right)$$ 

$$G_{i}$$

$$P\left(D\right)=\sum_{n=1}^3 P\left(D|G_{i}\right)P\left(G_{i}\right)$$

$$\iota_{i}=\frac{X_{i}\left(1-X_{1}\right)}{X_{1}\left(1-X_{i}\right)}$$

$$P\left(D\right)=XP\left(G_{1}\right)+\frac{X\iota_{2}}{1-X+X\iota_{2}}P\left(G_{2}\right)+\frac{X\iota_{3}}{1-X+X\iota_{3}}P\left(G_{3}\right)$$

Or, generally describing

$$P\left(D\right)=\sum_{o=1}^{∞}{\frac{X\iota_{o}}{1-X+X\iota_{o}}P\left(G_{o}\right)}$$

$$\sum_{o=1}^{∞}{\frac{X\iota_{o}}{1-X+X\iota_{o}}P\left(G_{o}\right)}-P\left(D\right)=0$$

$$\sum_{o=1}^{∞}{\frac{X\iota_{o}}{1-X+X\iota_{o}}P\left(G_{o}\right)}=P\left(D\right)$$

A stochastic model might be describe with a distribution of each 

$$\iota_{o} \in \left(0,∞\right)$$

positive only style of distribution constrainted by the definition of odds ratios

$P\left(D\right)\in B\left(\alpha,\beta\right)$ in the style of beta distributions

$$1-\sum_{o=1}^{∞}{\frac{X\iota_{o}}{1-X+X\iota_{o}}P\left(G_{o}\right)}=\frac{X\iota_{\omega}}{1-X+X\iota_{\omega}}P\left(G_{\omega}\right)$$

$$1-P(D)=\frac{X\iota_{\omega}}{1-X-X\iota_{\omega}}P(G_{\omega})$$

$$(1-X-X\iota_{\omega})(1-P(D))=X\iota_{\omega}P(G_{\omega})$$

$$(1-X)(1-P(D))-X\iota_{\omega}(1-P(D))=X\iota_{\omega}P(G_{\omega})$$

$$(1-X)(1-P(D))=X\iota_{\omega}-X\iota_{\omega}P(G_{\omega}$$

$$(1-X)(1-P(D))=\iota_{\omega}(X-XP(G_{\omega}))$$

$$\frac{(1-X)(1-P(D))}{(X-XP(G_{\omega}))}=\iota_{\omega}$$

$$\frac{(1-X)(1-P(D))}{X(1-P(G_{\omega}))}=\iota_{\omega}$$
