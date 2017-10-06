# Exercise 6: Machine Learning and Cognitive Services

## Content
This part focusses on the role of ML and Cognitive Services in innovative apps:
* Short recap: What is Machine Learning?
* ML-as-a-Service: Cognitive Services
* Microsoft’s related offerings in Azure (e.g. Azure Machine Learning, specialized storage systems)

## Material
This block contains presentations and demos/hands-on exercises. Slides about
* machine learning fundamentals,
* Microsoft Cognitive Services,
* Implications of ML on software development (e.g. required skills), and
* related Azure services (e.g. Azure Machine Learning, R, specialized storage systems)
will be provided. A step-by-step description of the following demos will be created:
* Cognitive Services: Try online demos
* Consume cognitive services in .NET
* Consume cognitive services in Java
Additionally, a collection of links to tutorials about Machine Learning in Azure will be added. Presenters who want to focus specifically on ML can make use of these resources.



## Requirements
* We will create a small Java application, therefore we need to fulfill some requirements.
* Install the [Java JDK](http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html)
* Install [Visual Studio Code](https://code.visualstudio.com) as our IDE.
* Install two Visual Studio Code extensions to get ready:
  * [Language Support for Java(TM) by Red Hat](https://marketplace.visualstudio.com/items?itemName=redhat.java)
  * [Java Debug Extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=vscjava.vscode-java-debug)



## Sample
### Scenario
* Analyze Support cases
  * Sentiment analysis
  * Language recognition
  * Image recognition / Text recognition
* Choose the best support engineer based on
  * Criticality (sentiment)
  * Language
  * Product group / Support plan



```
javac -cp lib/* -d bin src/App.java
java -cp lib/*;bin App
```
