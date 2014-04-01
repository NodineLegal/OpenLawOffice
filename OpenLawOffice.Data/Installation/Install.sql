--
-- PostgreSQL database dump
--

-- Dumped from database version 9.2.4
-- Dumped by pg_dump version 9.2.4
-- Started on 2014-03-30 09:11:37

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 197 (class 3079 OID 11727)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2179 (class 0 OID 0)
-- Dependencies: 197
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 168 (class 1259 OID 73178)
-- Name: area; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE area (
    id integer NOT NULL,
    parent_id integer,
    name text NOT NULL,
    description text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.area OWNER TO postgres;

--
-- TOC entry 169 (class 1259 OID 73184)
-- Name: area_acl; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE area_acl (
    id integer NOT NULL,
    security_area_id integer NOT NULL,
    user_id integer NOT NULL,
    allow_flags integer NOT NULL,
    deny_flags integer NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.area_acl OWNER TO postgres;

--
-- TOC entry 170 (class 1259 OID 73187)
-- Name: area_acl_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE area_acl_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.area_acl_id_seq OWNER TO postgres;

--
-- TOC entry 2180 (class 0 OID 0)
-- Dependencies: 170
-- Name: area_acl_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE area_acl_id_seq OWNED BY area_acl.id;


--
-- TOC entry 171 (class 1259 OID 73189)
-- Name: area_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE area_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.area_id_seq OWNER TO postgres;

--
-- TOC entry 2181 (class 0 OID 0)
-- Dependencies: 171
-- Name: area_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE area_id_seq OWNED BY area.id;


--
-- TOC entry 172 (class 1259 OID 73191)
-- Name: contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE contact (
    id integer NOT NULL,
    is_organization boolean NOT NULL,
    is_our_employee boolean NOT NULL,
    nickname text,
    generation text,
    display_name_prefix text,
    surname text,
    middle_name text,
    given_name text,
    initials text,
    display_name text NOT NULL,
    email1_display_name text,
    email1_email_address text,
    email2_display_name text,
    email2_email_address text,
    email3_display_name text,
    email3_email_address text,
    fax1_display_name text,
    fax1_fax_number text,
    fax2_display_name text,
    fax2_fax_number text,
    fax3_display_name text,
    fax3_fax_number text,
    address1_display_name text,
    address1_address_street text,
    address1_address_city text,
    address1_address_state_or_province text,
    address1_address_postal_code text,
    address1_address_country text,
    address1_address_country_code text,
    address1_address_post_office_box text,
    address2_display_name text,
    address2_address_street text,
    address2_address_city text,
    address2_address_state_or_province text,
    address2_address_postal_code text,
    address2_address_country text,
    address2_address_country_code text,
    address2_address_post_office_box text,
    address3_display_name text,
    address3_address_street text,
    address3_address_city text,
    address3_address_state_or_province text,
    address3_address_postal_code text,
    address3_address_country text,
    address3_address_country_code text,
    address3_address_post_office_box text,
    telephone1_display_name text,
    telephone1_telephone_number text,
    telephone2_display_name text,
    telephone2_telephone_number text,
    telephone3_display_name text,
    telephone3_telephone_number text,
    telephone4_display_name text,
    telephone4_telephone_number text,
    telephone5_display_name text,
    telephone5_telephone_number text,
    telephone6_display_name text,
    telephone6_telephone_number text,
    telephone7_display_name text,
    telephone7_telephone_number text,
    telephone8_display_name text,
    telephone8_telephone_number text,
    telephone9_display_name text,
    telephone9_telephone_number text,
    telephone10_display_name text,
    telephone10_telephone_number text,
    birthday timestamp without time zone,
    wedding timestamp without time zone,
    title text,
    company_name text,
    department_name text,
    office_location text,
    manager_name text,
    assistant_name text,
    profession text,
    spouse_name text,
    language text,
    instant_messaging_address text,
    personal_home_page text,
    business_home_page text,
    gender text,
    referred_by_name text,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.contact OWNER TO postgres;

--
-- TOC entry 173 (class 1259 OID 73197)
-- Name: contact_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE contact_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.contact_id_seq OWNER TO postgres;

--
-- TOC entry 2182 (class 0 OID 0)
-- Dependencies: 173
-- Name: contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE contact_id_seq OWNED BY contact.id;


--
-- TOC entry 174 (class 1259 OID 73199)
-- Name: matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter (
    title text NOT NULL,
    parent_id uuid,
    synopsis text NOT NULL,
    id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.matter OWNER TO postgres;

--
-- TOC entry 175 (class 1259 OID 73205)
-- Name: matter_contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter_contact (
    id integer NOT NULL,
    matter_id uuid NOT NULL,
    contact_id integer NOT NULL,
    role text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.matter_contact OWNER TO postgres;

--
-- TOC entry 176 (class 1259 OID 73211)
-- Name: matter_contact_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE matter_contact_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.matter_contact_id_seq OWNER TO postgres;

--
-- TOC entry 2183 (class 0 OID 0)
-- Dependencies: 176
-- Name: matter_contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE matter_contact_id_seq OWNED BY matter_contact.id;


--
-- TOC entry 177 (class 1259 OID 73213)
-- Name: matter_tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE matter_tag (
    id uuid NOT NULL,
    matter_id uuid NOT NULL,
    tag_category_id integer,
    tag text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.matter_tag OWNER TO postgres;

--
-- TOC entry 194 (class 1259 OID 73717)
-- Name: note; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE note (
    id uuid NOT NULL,
    title text NOT NULL,
    body text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.note OWNER TO postgres;

--
-- TOC entry 196 (class 1259 OID 73788)
-- Name: note_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE note_matter (
    id uuid NOT NULL,
    note_id uuid NOT NULL,
    matter_id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.note_matter OWNER TO postgres;

--
-- TOC entry 195 (class 1259 OID 73743)
-- Name: note_task; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE note_task (
    id uuid NOT NULL,
    note_id uuid NOT NULL,
    task_id bigint NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.note_task OWNER TO postgres;

--
-- TOC entry 178 (class 1259 OID 73219)
-- Name: responsible_user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE responsible_user (
    id integer NOT NULL,
    matter_id uuid NOT NULL,
    user_id integer NOT NULL,
    responsibility text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.responsible_user OWNER TO postgres;

--
-- TOC entry 179 (class 1259 OID 73225)
-- Name: responsible_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE responsible_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.responsible_user_id_seq OWNER TO postgres;

--
-- TOC entry 2184 (class 0 OID 0)
-- Dependencies: 179
-- Name: responsible_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE responsible_user_id_seq OWNED BY responsible_user.id;


--
-- TOC entry 180 (class 1259 OID 73227)
-- Name: secured_resource; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE secured_resource (
    id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.secured_resource OWNER TO postgres;

--
-- TOC entry 181 (class 1259 OID 73230)
-- Name: secured_resource_acl; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE secured_resource_acl (
    id uuid NOT NULL,
    secured_resource_id uuid NOT NULL,
    user_id integer NOT NULL,
    allow_flags integer NOT NULL,
    deny_flags integer NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.secured_resource_acl OWNER TO postgres;

--
-- TOC entry 182 (class 1259 OID 73233)
-- Name: tag_category; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE tag_category (
    id integer NOT NULL,
    name text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.tag_category OWNER TO postgres;

--
-- TOC entry 183 (class 1259 OID 73239)
-- Name: tag_category_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE tag_category_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tag_category_id_seq OWNER TO postgres;

--
-- TOC entry 2185 (class 0 OID 0)
-- Dependencies: 183
-- Name: tag_category_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE tag_category_id_seq OWNED BY tag_category.id;


--
-- TOC entry 184 (class 1259 OID 73241)
-- Name: task; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task (
    id bigint NOT NULL,
    title text NOT NULL,
    description text NOT NULL,
    projected_start timestamp without time zone,
    due_date timestamp without time zone,
    projected_end timestamp without time zone,
    actual_end timestamp without time zone,
    parent_id bigint,
    is_grouping_task boolean NOT NULL,
    sequential_predecessor_id bigint,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task OWNER TO postgres;

--
-- TOC entry 185 (class 1259 OID 73247)
-- Name: task_assigned_contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_assigned_contact (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    contact_id integer NOT NULL,
    assignment_type smallint DEFAULT 1 NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task_assigned_contact OWNER TO postgres;

--
-- TOC entry 186 (class 1259 OID 73251)
-- Name: task_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE task_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.task_id_seq OWNER TO postgres;

--
-- TOC entry 2186 (class 0 OID 0)
-- Dependencies: 186
-- Name: task_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE task_id_seq OWNED BY task.id;


--
-- TOC entry 187 (class 1259 OID 73253)
-- Name: task_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_matter (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    matter_id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task_matter OWNER TO postgres;

--
-- TOC entry 188 (class 1259 OID 73256)
-- Name: task_responsible_user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_responsible_user (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    user_id integer NOT NULL,
    responsibility text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task_responsible_user OWNER TO postgres;

--
-- TOC entry 189 (class 1259 OID 73262)
-- Name: task_tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_tag (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    tag_category_id integer,
    tag text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task_tag OWNER TO postgres;

--
-- TOC entry 190 (class 1259 OID 73268)
-- Name: task_time; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE task_time (
    id uuid NOT NULL,
    task_id bigint NOT NULL,
    time_id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.task_time OWNER TO postgres;

--
-- TOC entry 191 (class 1259 OID 73271)
-- Name: time; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "time" (
    id uuid NOT NULL,
    start timestamp without time zone NOT NULL,
    stop timestamp without time zone NOT NULL,
    worker_contact_id integer NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public."time" OWNER TO postgres;

--
-- TOC entry 192 (class 1259 OID 73274)
-- Name: user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE "user" (
    id integer NOT NULL,
    username text NOT NULL,
    password text NOT NULL,
    password_salt text NOT NULL,
    user_auth_token uuid,
    user_auth_token_expiry timestamp without time zone,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- TOC entry 193 (class 1259 OID 73280)
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.user_id_seq OWNER TO postgres;

--
-- TOC entry 2187 (class 0 OID 0)
-- Dependencies: 193
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE user_id_seq OWNED BY "user".id;


--
-- TOC entry 2022 (class 2604 OID 73282)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area ALTER COLUMN id SET DEFAULT nextval('area_id_seq'::regclass);


--
-- TOC entry 2023 (class 2604 OID 73283)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl ALTER COLUMN id SET DEFAULT nextval('area_acl_id_seq'::regclass);


--
-- TOC entry 2024 (class 2604 OID 73284)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact ALTER COLUMN id SET DEFAULT nextval('contact_id_seq'::regclass);


--
-- TOC entry 2025 (class 2604 OID 73285)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact ALTER COLUMN id SET DEFAULT nextval('matter_contact_id_seq'::regclass);


--
-- TOC entry 2026 (class 2604 OID 73286)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user ALTER COLUMN id SET DEFAULT nextval('responsible_user_id_seq'::regclass);


--
-- TOC entry 2027 (class 2604 OID 73287)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category ALTER COLUMN id SET DEFAULT nextval('tag_category_id_seq'::regclass);


--
-- TOC entry 2028 (class 2604 OID 73288)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task ALTER COLUMN id SET DEFAULT nextval('task_id_seq'::regclass);


--
-- TOC entry 2030 (class 2604 OID 73289)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "user" ALTER COLUMN id SET DEFAULT nextval('user_id_seq'::regclass);


--
-- TOC entry 2035 (class 2606 OID 73291)
-- Name: UQ_area_acl_Area_User; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "UQ_area_acl_Area_User" UNIQUE (security_area_id, user_id);


--
-- TOC entry 2051 (class 2606 OID 73293)
-- Name: UQ_secured_resource_acl_SecuredResource_User; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "UQ_secured_resource_acl_SecuredResource_User" UNIQUE (secured_resource_id, user_id);


--
-- TOC entry 2062 (class 2606 OID 73295)
-- Name: UQ_task_matter_Task_Matter; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "UQ_task_matter_Task_Matter" UNIQUE (task_id, matter_id);


--
-- TOC entry 2037 (class 2606 OID 73297)
-- Name: area_acl_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT area_acl_pkey PRIMARY KEY (id);


--
-- TOC entry 2032 (class 2606 OID 73299)
-- Name: area_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY area
    ADD CONSTRAINT area_pkey PRIMARY KEY (id);


--
-- TOC entry 2039 (class 2606 OID 73301)
-- Name: contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2043 (class 2606 OID 73303)
-- Name: matter_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT matter_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2041 (class 2606 OID 73305)
-- Name: matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2045 (class 2606 OID 73307)
-- Name: matter_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT matter_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 2082 (class 2606 OID 73792)
-- Name: note_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT note_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2078 (class 2606 OID 73724)
-- Name: note_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note
    ADD CONSTRAINT note_pkey PRIMARY KEY (id);


--
-- TOC entry 2080 (class 2606 OID 73747)
-- Name: note_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT note_task_pkey PRIMARY KEY (id);


--
-- TOC entry 2047 (class 2606 OID 73309)
-- Name: responsible_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT responsible_user_pkey PRIMARY KEY (id);


--
-- TOC entry 2053 (class 2606 OID 73311)
-- Name: secured_resource_acl_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT secured_resource_acl_pkey PRIMARY KEY (id);


--
-- TOC entry 2049 (class 2606 OID 73313)
-- Name: secured_resource_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT secured_resource_pkey PRIMARY KEY (id);


--
-- TOC entry 2055 (class 2606 OID 73315)
-- Name: tag_category_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT tag_category_pkey PRIMARY KEY (id);


--
-- TOC entry 2060 (class 2606 OID 73317)
-- Name: task_assigned_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT task_assigned_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2064 (class 2606 OID 73319)
-- Name: task_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT task_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2058 (class 2606 OID 73321)
-- Name: task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task
    ADD CONSTRAINT task_pkey PRIMARY KEY (id);


--
-- TOC entry 2066 (class 2606 OID 73323)
-- Name: task_responsible_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT task_responsible_user_pkey PRIMARY KEY (id);


--
-- TOC entry 2068 (class 2606 OID 73325)
-- Name: task_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT task_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 2070 (class 2606 OID 73327)
-- Name: task_time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT task_time_pkey PRIMARY KEY (id);


--
-- TOC entry 2072 (class 2606 OID 73329)
-- Name: time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT time_pkey PRIMARY KEY (id);


--
-- TOC entry 2076 (class 2606 OID 73331)
-- Name: user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- TOC entry 2033 (class 1259 OID 73332)
-- Name: uidx_area_name; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_area_name ON area USING btree (name);


--
-- TOC entry 2056 (class 1259 OID 73333)
-- Name: uidx_tagcategory_name; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_tagcategory_name ON tag_category USING btree (name);


--
-- TOC entry 2073 (class 1259 OID 73334)
-- Name: uidx_user_userauthtoken; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_user_userauthtoken ON "user" USING btree (user_auth_token);


--
-- TOC entry 2074 (class 1259 OID 73335)
-- Name: uidx_user_username; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_user_username ON "user" USING btree (username);


--
-- TOC entry 2087 (class 2606 OID 73336)
-- Name: FK_area_acl_area_SecurityAreaId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_area_SecurityAreaId" FOREIGN KEY (security_area_id) REFERENCES area(id);


--
-- TOC entry 2088 (class 2606 OID 73341)
-- Name: FK_area_acl_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2089 (class 2606 OID 73346)
-- Name: FK_area_acl_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2090 (class 2606 OID 73351)
-- Name: FK_area_acl_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2091 (class 2606 OID 73356)
-- Name: FK_area_acl_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_UserId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2083 (class 2606 OID 73361)
-- Name: FK_area_area_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_area_ParentId" FOREIGN KEY (parent_id) REFERENCES area(id);


--
-- TOC entry 2084 (class 2606 OID 73366)
-- Name: FK_area_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2085 (class 2606 OID 73371)
-- Name: FK_area_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2086 (class 2606 OID 73376)
-- Name: FK_area_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2092 (class 2606 OID 73381)
-- Name: FK_contact_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2093 (class 2606 OID 73386)
-- Name: FK_contact_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2094 (class 2606 OID 73391)
-- Name: FK_contact_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2099 (class 2606 OID 73396)
-- Name: FK_matter_contact_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2100 (class 2606 OID 73401)
-- Name: FK_matter_contact_user_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2101 (class 2606 OID 73406)
-- Name: FK_matter_contact_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2102 (class 2606 OID 73411)
-- Name: FK_matter_contact_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2103 (class 2606 OID 73416)
-- Name: FK_matter_contact_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2095 (class 2606 OID 73421)
-- Name: FK_matter_matter_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_matter_ParentId" FOREIGN KEY (parent_id) REFERENCES matter(id);


--
-- TOC entry 2104 (class 2606 OID 73426)
-- Name: FK_matter_tag_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2105 (class 2606 OID 73431)
-- Name: FK_matter_tag_tag_category_TagCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_tag_category_TagCategoryId" FOREIGN KEY (tag_category_id) REFERENCES tag_category(id);


--
-- TOC entry 2106 (class 2606 OID 73436)
-- Name: FK_matter_tag_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2107 (class 2606 OID 73441)
-- Name: FK_matter_tag_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2108 (class 2606 OID 73446)
-- Name: FK_matter_tag_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2096 (class 2606 OID 73451)
-- Name: FK_matter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2097 (class 2606 OID 73456)
-- Name: FK_matter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2098 (class 2606 OID 73461)
-- Name: FK_matter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2171 (class 2606 OID 73813)
-- Name: FK_note_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2170 (class 2606 OID 73808)
-- Name: FK_note_matter_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2167 (class 2606 OID 73793)
-- Name: FK_note_matter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2168 (class 2606 OID 73798)
-- Name: FK_note_matter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2169 (class 2606 OID 73803)
-- Name: FK_note_matter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2165 (class 2606 OID 73833)
-- Name: FK_note_task_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2166 (class 2606 OID 73838)
-- Name: FK_note_task_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2162 (class 2606 OID 73818)
-- Name: FK_note_task_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2163 (class 2606 OID 73823)
-- Name: FK_note_task_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2164 (class 2606 OID 73828)
-- Name: FK_note_task_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2159 (class 2606 OID 73773)
-- Name: FK_note_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note
    ADD CONSTRAINT "FK_note_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2160 (class 2606 OID 73778)
-- Name: FK_note_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note
    ADD CONSTRAINT "FK_note_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2161 (class 2606 OID 73783)
-- Name: FK_note_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note
    ADD CONSTRAINT "FK_note_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2109 (class 2606 OID 73466)
-- Name: FK_responsible_user_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2110 (class 2606 OID 73471)
-- Name: FK_responsible_user_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2111 (class 2606 OID 73476)
-- Name: FK_responsible_user_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2112 (class 2606 OID 73481)
-- Name: FK_responsible_user_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2113 (class 2606 OID 73486)
-- Name: FK_responsible_user_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_UserId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2117 (class 2606 OID 73491)
-- Name: FK_secured_resource_acl_secured_resource_SecuredResourceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_secured_resource_SecuredResourceId" FOREIGN KEY (secured_resource_id) REFERENCES secured_resource(id);


--
-- TOC entry 2118 (class 2606 OID 73496)
-- Name: FK_secured_resource_acl_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2119 (class 2606 OID 73501)
-- Name: FK_secured_resource_acl_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2120 (class 2606 OID 73506)
-- Name: FK_secured_resource_acl_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2121 (class 2606 OID 73511)
-- Name: FK_secured_resource_acl_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_UserId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2114 (class 2606 OID 73516)
-- Name: FK_secured_resource_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT "FK_secured_resource_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2115 (class 2606 OID 73521)
-- Name: FK_secured_resource_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT "FK_secured_resource_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2116 (class 2606 OID 73526)
-- Name: FK_secured_resource_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT "FK_secured_resource_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2122 (class 2606 OID 73531)
-- Name: FK_tag_category_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT "FK_tag_category_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2123 (class 2606 OID 73536)
-- Name: FK_tag_category_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT "FK_tag_category_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2124 (class 2606 OID 73541)
-- Name: FK_tag_category_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT "FK_tag_category_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2130 (class 2606 OID 73546)
-- Name: FK_task_assigned_contact_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2131 (class 2606 OID 73551)
-- Name: FK_task_assigned_contact_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2132 (class 2606 OID 73556)
-- Name: FK_task_assigned_contact_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2133 (class 2606 OID 73561)
-- Name: FK_task_assigned_contact_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2134 (class 2606 OID 73566)
-- Name: FK_task_assigned_contact_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2135 (class 2606 OID 73571)
-- Name: FK_task_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2136 (class 2606 OID 73576)
-- Name: FK_task_matter_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2137 (class 2606 OID 73581)
-- Name: FK_task_matter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2138 (class 2606 OID 73586)
-- Name: FK_task_matter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2139 (class 2606 OID 73591)
-- Name: FK_task_matter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2140 (class 2606 OID 73596)
-- Name: FK_task_responsible_user_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2141 (class 2606 OID 73601)
-- Name: FK_task_responsible_user_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2142 (class 2606 OID 73606)
-- Name: FK_task_responsible_user_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2143 (class 2606 OID 73611)
-- Name: FK_task_responsible_user_user_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_MatterId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2144 (class 2606 OID 73616)
-- Name: FK_task_responsible_user_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2145 (class 2606 OID 73621)
-- Name: FK_task_tag_tag_category_TagCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_tag_category_TagCategoryId" FOREIGN KEY (tag_category_id) REFERENCES tag_category(id);


--
-- TOC entry 2146 (class 2606 OID 73626)
-- Name: FK_task_tag_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2147 (class 2606 OID 73631)
-- Name: FK_task_tag_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2148 (class 2606 OID 73636)
-- Name: FK_task_tag_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2149 (class 2606 OID 73641)
-- Name: FK_task_tag_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2125 (class 2606 OID 73646)
-- Name: FK_task_task_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_ParentId" FOREIGN KEY (parent_id) REFERENCES task(id);


--
-- TOC entry 2126 (class 2606 OID 73651)
-- Name: FK_task_task_SequentialPredecessorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_SequentialPredecessorId" FOREIGN KEY (sequential_predecessor_id) REFERENCES task(id);


--
-- TOC entry 2150 (class 2606 OID 73656)
-- Name: FK_task_time_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2151 (class 2606 OID 73661)
-- Name: FK_task_time_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2152 (class 2606 OID 73666)
-- Name: FK_task_time_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2153 (class 2606 OID 73671)
-- Name: FK_task_time_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2154 (class 2606 OID 73676)
-- Name: FK_task_time_user_TimeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_TimeId" FOREIGN KEY (time_id) REFERENCES "time"(id);


--
-- TOC entry 2127 (class 2606 OID 73681)
-- Name: FK_task_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2128 (class 2606 OID 73686)
-- Name: FK_task_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2129 (class 2606 OID 73691)
-- Name: FK_task_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2155 (class 2606 OID 73696)
-- Name: FK_time_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2156 (class 2606 OID 73701)
-- Name: FK_time_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2157 (class 2606 OID 73706)
-- Name: FK_time_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2158 (class 2606 OID 73711)
-- Name: FK_time_user_WorkerContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_WorkerContactId" FOREIGN KEY (worker_contact_id) REFERENCES contact(id);


--
-- TOC entry 2178 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2014-03-30 09:11:38

--
-- PostgreSQL database dump complete
--

