pipeline { 
    agent any 
    options {
        skipStagesAfterUnstable()
    }
    stages {
        stage('Build') { 
            steps { 
                sh 'dotnet build' 
            }
        }
        stage('Test'){
            steps { 
            }
        }
        stage('Deploy') {
            steps {
            }
        }
    }
}
