export interface User {
  id: number;
  userName: string;
  knownAs: string;
  age: number;
  gender: string;
  created: Date;
  lastActive: Date;
  photoUrl: string;
  city: string;
  country: string;

  // This is optional that is why we use the 'elvies' or '?' operator
  // Optional properties should come after the required properties or else you will get
  // an error
  interest?: string;
  introduction?: string;
  lookingFor?: string;
  roles?: string[];
}
