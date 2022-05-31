using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NABLA.sim
{
    /// <summary>
    /// A class that implements the core functionality of DC Modfied Nodal Analysis
    /// </summary>
    public class ModfiedNodalAnalysis
    {
        
        //***** Properties *****

        /// <summary>
        /// The circuit upon which the analysis is being performed
        /// </summary>
        private Circuit _circuit;

        /// <summary>
        /// Backing field for the formatted result from the analysis
        /// </summary>
        private string _result;
        /// <summary>
        /// Formatted result from the analysis
        /// </summary>
        public string Result { get { return _result; } }

        //***** Matricies *****

        /// <summary>
        /// An n by n matrix describing connections of resistors, capacitors, and vccs elements
        /// </summary>
        private Matrix<double> _gMatrix;
        /// <summary>
        /// An n by m matrix describing nodes and voltage sources
        /// </summary>
        private Matrix<double> _bMatrix;
        /// <summary>
        /// An n by m matrix describing nodes and voltage sources
        /// </summary>
        private Matrix<double> _cMatrix;
        /// <summary>
        /// An m by n matrix
        /// </summary>
        private Matrix<double> _dMatrix;
        /// <summary>
        /// An (m+n) by 1 matrix storing unknowns
        /// </summary>
        private Matrix<double> _xMatrix;
        /// <summary>
        /// An (m + n) by 1 matrix holding known values of current and voltage sources
        /// </summary>
        private Matrix<double> _zMatrix;
        /// <summary>
        /// An (m + n) by (m + n) matrix that is formed out of matrix G, B, C, and D 
        /// </summary>
        private Matrix<double> _aMatrix;


        //***** Counts of various elements and values relating to the circuit *****

        private int _numberRLC = 0;
        private int _numberInductors = 0;
        private int _numberVoltageSource = 0;
        private int _numberCurrentSource = 0;
        private int _numberCurrentUnknowns = 0;
        private int _numberBranches = 0;


        //***** Constructors *****

        public ModfiedNodalAnalysis(Circuit circuit)
        {
            _circuit = circuit;
        }

        //***** Utility ******

        /// <summary>
        /// Iterates over the circuit and counts the number of each element
        /// </summary>
        private void GetElementCounts()
        {
            foreach (KeyValuePair<string, Connector> element in _circuit)
            {
                //TODO: add the rest of the components
                switch (element.Key[0])
                {
                    case 'R':
                        _numberRLC++;
                        _numberBranches++;
                        break;

                    case 'L':
                        _numberRLC++;
                        _numberBranches++;
                        _numberInductors++;
                        break;

                    case 'C':
                        _numberRLC++;
                        _numberBranches++;
                        break;

                    case 'V':
                        _numberVoltageSource++;
                        _numberBranches++;
                        break;

                    default:
                        break;
                }
                _numberCurrentUnknowns = _numberVoltageSource + _numberInductors;
            }
        }

        //***** Matrix Builders *****

        /// <summary>
        /// Build matrix G, an n by n matrix describing connections of resistors, capacitors and vccs elements
        /// </summary>
        /// <returns>An n by n sparse matrix of doubles</returns>
        private Matrix<double> buildMatrixG()
        {
            //assign an n by n sparse matrix, this is what we'll build
            Matrix<double> matrix = Matrix<double>.Build.Sparse(_circuit.GetNodeCount(), _circuit.GetNodeCount());

            //iterate over every dictionary element describing components in the circuit
            foreach (KeyValuePair<string, Connector> element in _circuit)
            {
                int node1 = element.Value.GetNodes()[0];
                int node2 = element.Value.GetNodes()[1];
                //the value of inductance
                double g = 0;

                //calculate the inductance for the element
                switch (element.Key[0])
                {
                    //TODO: gonna be a few slow reciprocals here, replace with some math functionality
                    case 'R':
                        g = 1 / element.Value.GetParameter("Resistance");
                        break;
                    //TODO: implement every other linear component type

                    default:
                        break;
                }

                //switching on the first char as this signifies the type
                switch (element.Key[0])
                {
                    case 'R':
                        if (node1 != 0 && node2 != 0)
                        {
                            matrix[node1 - 1, node2 - 1] += -g;
                            matrix[node2 - 1, node1 - 1] += -g;
                        }
                        if (node1 != 0)
                        {
                            matrix[node1 - 1, node1 - 1] += g;
                        }
                        if (node2 != 0)
                        {
                            matrix[node2 - 1, node2 - 1] += g;
                        }
                        break;

                    default:
                        break;
                }
            }
            return matrix;

        }

        /// <summary>
        /// Build matrix B, an n by m matrix describing nodes and voltage sources
        /// </summary>
        /// <returns>An n by m sparse matrix of doubles</returns>
        private Matrix<double> buildMatrixB()
        {
            //assign an n by m (number of unknown currents) matrix to build into
            Matrix<double> matrix = Matrix<double>.Build.Sparse(_circuit.GetNodeCount(), _numberCurrentUnknowns);

            int _sourceNumber = 0;

            //enumerate over each circuit element
            foreach (KeyValuePair<string, Connector> element in _circuit)
            {
                int node1 = element.Value.GetNodes()[0];
                int node2 = element.Value.GetNodes()[1];

                //switch on first char as this is element type
                switch (element.Key[0])
                {
                    //voltage source
                    case 'V':
                        if (_numberCurrentUnknowns > 1)
                        {
                            if (node1 != 0)
                            {
                                matrix[node1 - 1, _sourceNumber] = -1;
                            }
                            if (node2 != 0)
                            {
                                matrix[node2 - 1, _sourceNumber] = 1;
                            }
                            _sourceNumber++;
                            break;  
                        }
                        else
                        {
                            if (node1 != 0)
                            {
                                for (int i = 0; i < _numberCurrentUnknowns; i++)
                                {
                                    matrix[node1 - 1, i] = -1;
                                }
                            }
                            if (node2 != 0)
                            {
                                for (int i = 0; i < _numberCurrentUnknowns; i++)
                                {
                                    matrix[node2 - 1, i] = 1;
                                }
                            }
                            _sourceNumber++;
                            break;
                        }
                    //TODO: add all the other source types
                    default:
                        break;

                }
            }
            return matrix;
        }

        /// <summary>
        /// Build matrix C, an n by m matrix describing nodes and voltage sources
        /// </summary>
        /// <returns>An n by m sparse matrix of doubles</returns>
        private Matrix<double> buildMatrixC()
        {
            //assign an n by m (number of unknown currents) matrix to build into
            Matrix<double> matrix = Matrix<double>.Build.Sparse(_numberCurrentUnknowns, _circuit.GetNodeCount());

            //needed to get the correct matrix index
            int _sourceNumber = 0;

            //enumerate over each element in circuit
            foreach (KeyValuePair<string, Connector> element in _circuit)
            {
                int node1 = element.Value.GetNodes()[0];
                int node2 = element.Value.GetNodes()[1];

                //switch on first char as this is element type
                switch (element.Key[0])
                {
                    case 'V':
                        if (_numberCurrentUnknowns > 1)
                        {
                            if (node1 != 0)
                            {
                                matrix[_sourceNumber, node1 - 1] = -1;
                            }
                            if (node2 != 0)
                            {
                                matrix[_sourceNumber, node2 - 1] = 1;
                            }
                            _sourceNumber++;
                            break;
                        }
                        else
                        {
                            if (node1 != 0)
                            {
                                for (int i = 0; i < _numberCurrentUnknowns; i++)
                                {
                                    matrix[i, node1 - 1] = -1;
                                }
                            }
                            if (node2 != 0)
                            {
                                for (int i = 0; i < _numberCurrentUnknowns; i++)
                                {
                                    matrix[i, node2 - 1] = 1;
                                }
                            }
                            _sourceNumber++;
                            break;
                        }
                    default:
                        break;

                }
            }
            return matrix;
        }

        /// <summary>
        /// An m by m Matrix 
        /// </summary>
        /// <returns>An m by m sparse matrix of doubles</returns>
        private Matrix<double> buildMatrixD()
        {
            //assign an m by m matrix that we'll build into
            Matrix<double> matrix = Matrix<double>.Build.Sparse(_numberCurrentUnknowns, _numberCurrentUnknowns);


            int _sourceNumber = 0;

            //enumerate over each circuit element
            foreach (KeyValuePair<string, Connector> element in _circuit)
            {
                int node1 = element.Value.GetNodes()[0];
                int node2 = element.Value.GetNodes()[1];

                //switch on first char, the element type
                switch (element.Key[0])
                {
                    case 'V':
                        _sourceNumber++;
                        break;

                    default:
                        break;
                }
            }
            return matrix;
        }

        /// <summary>
        /// Build matrix X, an (m + n) by 1 matrix that holds unknown values
        /// </summary>
        /// <returns>An (m + n) by 1 sparse matrix of doubles</returns>
        private Matrix<double> buildMatrixX()
        {
            //this matrix is empty just needs to be sized correctly for the circuit
            Matrix<double> matrix = Matrix<double>.Build.Sparse((_circuit.GetNodeCount() + _numberCurrentUnknowns), 1);
            return matrix;
        }

        /// <summary>
        /// Build matrix Z, an (m + n) by 1 matrix that holds known values of voltage and current sources
        /// </summary>
        /// <returns>An (m + n) by 1 sparse matrix of doubles</returns>
        private Matrix<double> buildMatrixZ()
        {
            //two matricies get built, they then get combined into z
            Matrix<double> matrixI = Matrix<double>.Build.Sparse(_circuit.GetNodeCount(), 1);
            Matrix<double> matrixEv = Matrix<double>.Build.Sparse(_numberCurrentUnknowns, 1);

            //start with the I matrix
            foreach (KeyValuePair<string, Connector> element in _circuit)
            {
                int node1 = element.Value.GetNodes()[0];
                int node2 = element.Value.GetNodes()[1];

                if (element.Key[0] == 'I')
                {
                    if (node1 != 0)
                    {
                        //TODO: this is not the right parameter refrence but idk what to put sooo...
                        matrixI[node1 - 1, 0] += -element.Value.GetParameter("Inductance");
                    }
                    else if (node2 != 0)
                    {
                        //TODO: same as above - this is not the right parameter refrence but idk what to put sooo...
                        matrixI[node2 - 1, 0] += element.Value.GetParameter("Inductance");
                    }
                }
            }

            //now the Ev matrix
            int _sourceNumber = 0;
            foreach (KeyValuePair<string, Connector> element in _circuit)
            {
                int node1 = element.Value.GetNodes()[0];
                int node2 = element.Value.GetNodes()[1];

                if (element.Key[0] == 'V')
                {
                    matrixEv[_sourceNumber, 0] = element.Value.GetParameter("Voltage");
                }
                _sourceNumber++;
            }

            // And then combine the two together
            //TODO: i think this works... edit, this works

            Matrix<double> matrixZ = Matrix<double>.Build.Sparse((_circuit.GetNodeCount() + _numberCurrentUnknowns), 1);

            for (int i = 0; i < _circuit.GetNodeCount(); i++)
            {
                matrixZ[i, 0] = matrixI[i, 0];
            }
            for (int i = 0; i < _numberCurrentUnknowns; i++)
            {
                matrixZ[_circuit.GetNodeCount() + i, 0] = matrixEv[i, 0];
            }

            return matrixZ;
        }

        /// <summary>
        /// Build matrix A, an (m + n) by (m + n) matrix that is formed out of matrix G, B, C, and D 
        /// </summary>
        /// <returns>An (m + n) by (m + n) sparse matrix of doubles</returns>
        private Matrix<double> buildMatrixA()
        {   
            //An (m+n) by (m+n) matrix to build into
            Matrix<double> matrix = Matrix<double>.Build.Sparse((_circuit.GetNodeCount() + _numberCurrentUnknowns), (_circuit.GetNodeCount() + _numberCurrentUnknowns));

            //copy in the g matrix
            for (int i = 0; i < _circuit.GetNodeCount(); i++)
            {
                for (int q = 0; q < _circuit.GetNodeCount(); q++)
                {
                    matrix[i, q] = _gMatrix[i, q];
                    Console.WriteLine(_gMatrix[i, q]);
                }
            }

            //TODO: beware, an undocumented mess follows. Needs tidying and documenting

            if (_numberCurrentUnknowns > 1)
            {
                for (int i = 0; i < _circuit.GetNodeCount(); i++)
                {
                    for (int q = 0; q < _numberCurrentUnknowns; q++)
                    {
                        matrix[i, _circuit.GetNodeCount() + q] = _bMatrix[i, q];
                        matrix[_circuit.GetNodeCount() + q, i] = _cMatrix[q, i];
                    }
                }

                for (int i = 0; i < _numberCurrentUnknowns; i++)
                {
                    for (int q = 0; q < _numberCurrentUnknowns; q++)
                    {
                        matrix[_circuit.GetNodeCount() + i, _circuit.GetNodeCount() + q] = _dMatrix[i, q];
                    }
                }
            }
            else if (_numberCurrentUnknowns == 1)
            {
                for (int i = 0; i < _circuit.GetNodeCount(); i++)
                {
                    matrix[i, _circuit.GetNodeCount()] = _bMatrix[i, 0];
                    matrix[_circuit.GetNodeCount(), i] = _cMatrix[i, 0];
                }
            }
            return matrix;
        }

        //***** Solvers *****

        /// <summary>
        /// Perform a linear solve of the circuit using MNA
        /// </summary>
        /// <returns>An (m + n) by 1 sparse matrix of doubles holding solved for values</returns>
        public Matrix<double> Solve()
        {
            //get everything in order first, the order does matter as well
            GetElementCounts();
            _gMatrix = buildMatrixG();
            _bMatrix = buildMatrixB();
            _cMatrix = buildMatrixC();
            _dMatrix = buildMatrixD();
            _xMatrix = buildMatrixX();
            _zMatrix = buildMatrixZ();
            _aMatrix = buildMatrixA();

            //this is the actual solvey bit - not that impressive really
            _xMatrix = _aMatrix.Inverse() * _zMatrix;
            
            //clear out the result
            _result = "";

            //go over and add some labels will sticking the value in the result variable
            for (int i = 0; i < _numberCurrentUnknowns; i++)
            {
                _result += "Source " + i + ": " + Math.Round(_xMatrix[i, 0], 4) + "A\n"; 
            }
            for (int i = 0; i < _circuit.GetNodeCount(); i++)
            {
                _result += "Node " + i + ": " + Math.Round(_xMatrix[i + _numberCurrentUnknowns, 0], 4) + "v\n";
            }

            return _xMatrix;
        }
    }
}
