--
-- PostgreSQL database dump
--

-- Dumped from database version 9.2.4
-- Dumped by pg_dump version 9.2.4
-- Started on 2014-06-13 20:11:58

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 212 (class 3079 OID 11727)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2326 (class 0 OID 0)
-- Dependencies: 212
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 168 (class 1259 OID 114050)
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
-- TOC entry 169 (class 1259 OID 114056)
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
-- TOC entry 170 (class 1259 OID 114059)
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
-- TOC entry 2327 (class 0 OID 0)
-- Dependencies: 170
-- Name: area_acl_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE area_acl_id_seq OWNED BY area_acl.id;


--
-- TOC entry 171 (class 1259 OID 114061)
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
-- TOC entry 2328 (class 0 OID 0)
-- Dependencies: 171
-- Name: area_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE area_id_seq OWNED BY area.id;


--
-- TOC entry 172 (class 1259 OID 114063)
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
-- TOC entry 173 (class 1259 OID 114069)
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
-- TOC entry 2329 (class 0 OID 0)
-- Dependencies: 173
-- Name: contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE contact_id_seq OWNED BY contact.id;


--
-- TOC entry 174 (class 1259 OID 114071)
-- Name: document; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE document (
    id uuid NOT NULL,
    title text NOT NULL,
    date timestamp without time zone,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.document OWNER TO postgres;

--
-- TOC entry 175 (class 1259 OID 114077)
-- Name: document_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE document_matter (
    id uuid NOT NULL,
    document_id uuid NOT NULL,
    matter_id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.document_matter OWNER TO postgres;

--
-- TOC entry 176 (class 1259 OID 114080)
-- Name: document_task; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE document_task (
    id uuid NOT NULL,
    document_id uuid NOT NULL,
    task_id bigint NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.document_task OWNER TO postgres;

--
-- TOC entry 177 (class 1259 OID 114083)
-- Name: elmah_error_sequence; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE elmah_error_sequence
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.elmah_error_sequence OWNER TO postgres;

--
-- TOC entry 178 (class 1259 OID 114085)
-- Name: elmah_error; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE elmah_error (
    errorid character(36) NOT NULL,
    application character varying(60) NOT NULL,
    host character varying(50) NOT NULL,
    type character varying(100) NOT NULL,
    source character varying(60) NOT NULL,
    message character varying(500) NOT NULL,
    "User" character varying(50) NOT NULL,
    statuscode integer NOT NULL,
    timeutc timestamp without time zone NOT NULL,
    sequence integer DEFAULT nextval('elmah_error_sequence'::regclass) NOT NULL,
    allxml text NOT NULL
);


ALTER TABLE public.elmah_error OWNER TO postgres;

--
-- TOC entry 179 (class 1259 OID 114092)
-- Name: event; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event (
    id uuid NOT NULL,
    title text NOT NULL,
    allday boolean NOT NULL,
    start timestamp without time zone NOT NULL,
    "end" timestamp without time zone,
    location text,
    description text,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.event OWNER TO postgres;

--
-- TOC entry 180 (class 1259 OID 114098)
-- Name: event_assigned_contact; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_assigned_contact (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    contact_id integer NOT NULL,
    role text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.event_assigned_contact OWNER TO postgres;

--
-- TOC entry 181 (class 1259 OID 114104)
-- Name: event_matter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_matter (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    matter_id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.event_matter OWNER TO postgres;

--
-- TOC entry 182 (class 1259 OID 114107)
-- Name: event_note; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_note (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    note_id uuid NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.event_note OWNER TO postgres;

--
-- TOC entry 183 (class 1259 OID 114110)
-- Name: event_responsible_user; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_responsible_user (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    user_id integer NOT NULL,
    responsibility text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.event_responsible_user OWNER TO postgres;

--
-- TOC entry 184 (class 1259 OID 114116)
-- Name: event_tag; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_tag (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    tag_category_id integer,
    tag text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.event_tag OWNER TO postgres;

--
-- TOC entry 185 (class 1259 OID 114122)
-- Name: event_task; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE event_task (
    id uuid NOT NULL,
    event_id uuid NOT NULL,
    task_id bigint NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.event_task OWNER TO postgres;

--
-- TOC entry 186 (class 1259 OID 114125)
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
-- TOC entry 187 (class 1259 OID 114131)
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
-- TOC entry 188 (class 1259 OID 114137)
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
-- TOC entry 2330 (class 0 OID 0)
-- Dependencies: 188
-- Name: matter_contact_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE matter_contact_id_seq OWNED BY matter_contact.id;


--
-- TOC entry 189 (class 1259 OID 114139)
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
-- TOC entry 190 (class 1259 OID 114145)
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
-- TOC entry 191 (class 1259 OID 114151)
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
-- TOC entry 192 (class 1259 OID 114154)
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
-- TOC entry 193 (class 1259 OID 114157)
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
-- TOC entry 194 (class 1259 OID 114163)
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
-- TOC entry 2331 (class 0 OID 0)
-- Dependencies: 194
-- Name: responsible_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE responsible_user_id_seq OWNED BY responsible_user.id;


--
-- TOC entry 195 (class 1259 OID 114165)
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
-- TOC entry 196 (class 1259 OID 114168)
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
-- TOC entry 197 (class 1259 OID 114171)
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
-- TOC entry 198 (class 1259 OID 114177)
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
-- TOC entry 2332 (class 0 OID 0)
-- Dependencies: 198
-- Name: tag_category_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE tag_category_id_seq OWNED BY tag_category.id;


--
-- TOC entry 199 (class 1259 OID 114179)
-- Name: tag_filter; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE tag_filter (
    id bigint NOT NULL,
    user_id integer NOT NULL,
    category text,
    tag text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.tag_filter OWNER TO postgres;

--
-- TOC entry 200 (class 1259 OID 114185)
-- Name: tag_filter_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE tag_filter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tag_filter_id_seq OWNER TO postgres;

--
-- TOC entry 2333 (class 0 OID 0)
-- Dependencies: 200
-- Name: tag_filter_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE tag_filter_id_seq OWNED BY tag_filter.id;


--
-- TOC entry 201 (class 1259 OID 114187)
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
    utc_disabled timestamp without time zone,
    active boolean NOT NULL
);


ALTER TABLE public.task OWNER TO postgres;

--
-- TOC entry 202 (class 1259 OID 114193)
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
-- TOC entry 203 (class 1259 OID 114197)
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
-- TOC entry 2334 (class 0 OID 0)
-- Dependencies: 203
-- Name: task_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE task_id_seq OWNED BY task.id;


--
-- TOC entry 204 (class 1259 OID 114199)
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
-- TOC entry 205 (class 1259 OID 114202)
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
-- TOC entry 206 (class 1259 OID 114208)
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
-- TOC entry 207 (class 1259 OID 114214)
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
-- TOC entry 208 (class 1259 OID 114217)
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
    utc_disabled timestamp without time zone,
    details text
);


ALTER TABLE public."time" OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 114220)
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
-- TOC entry 210 (class 1259 OID 114226)
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
-- TOC entry 2335 (class 0 OID 0)
-- Dependencies: 210
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE user_id_seq OWNED BY "user".id;


--
-- TOC entry 211 (class 1259 OID 114228)
-- Name: version; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE version (
    id uuid NOT NULL,
    document_id uuid NOT NULL,
    version_number integer NOT NULL,
    mime text NOT NULL,
    filename text NOT NULL,
    extension text NOT NULL,
    size bigint NOT NULL,
    md5 text NOT NULL,
    created_by_user_id integer NOT NULL,
    modified_by_user_id integer NOT NULL,
    disabled_by_user_id integer,
    utc_created timestamp without time zone NOT NULL,
    utc_modified timestamp without time zone NOT NULL,
    utc_disabled timestamp without time zone
);


ALTER TABLE public.version OWNER TO postgres;

--
-- TOC entry 2087 (class 2604 OID 114234)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area ALTER COLUMN id SET DEFAULT nextval('area_id_seq'::regclass);


--
-- TOC entry 2088 (class 2604 OID 114235)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl ALTER COLUMN id SET DEFAULT nextval('area_acl_id_seq'::regclass);


--
-- TOC entry 2089 (class 2604 OID 114236)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact ALTER COLUMN id SET DEFAULT nextval('contact_id_seq'::regclass);


--
-- TOC entry 2091 (class 2604 OID 114237)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact ALTER COLUMN id SET DEFAULT nextval('matter_contact_id_seq'::regclass);


--
-- TOC entry 2092 (class 2604 OID 114238)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user ALTER COLUMN id SET DEFAULT nextval('responsible_user_id_seq'::regclass);


--
-- TOC entry 2093 (class 2604 OID 114239)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category ALTER COLUMN id SET DEFAULT nextval('tag_category_id_seq'::regclass);


--
-- TOC entry 2094 (class 2604 OID 114240)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_filter ALTER COLUMN id SET DEFAULT nextval('tag_filter_id_seq'::regclass);


--
-- TOC entry 2095 (class 2604 OID 114241)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task ALTER COLUMN id SET DEFAULT nextval('task_id_seq'::regclass);


--
-- TOC entry 2097 (class 2604 OID 114242)
-- Name: id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "user" ALTER COLUMN id SET DEFAULT nextval('user_id_seq'::regclass);


--
-- TOC entry 2102 (class 2606 OID 114244)
-- Name: UQ_area_acl_Area_User; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "UQ_area_acl_Area_User" UNIQUE (security_area_id, user_id);


--
-- TOC entry 2147 (class 2606 OID 114246)
-- Name: UQ_secured_resource_acl_SecuredResource_User; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "UQ_secured_resource_acl_SecuredResource_User" UNIQUE (secured_resource_id, user_id);


--
-- TOC entry 2160 (class 2606 OID 114248)
-- Name: UQ_task_matter_Task_Matter; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "UQ_task_matter_Task_Matter" UNIQUE (task_id, matter_id);


--
-- TOC entry 2104 (class 2606 OID 114250)
-- Name: area_acl_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT area_acl_pkey PRIMARY KEY (id);


--
-- TOC entry 2099 (class 2606 OID 114252)
-- Name: area_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY area
    ADD CONSTRAINT area_pkey PRIMARY KEY (id);


--
-- TOC entry 2106 (class 2606 OID 114254)
-- Name: contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2110 (class 2606 OID 114256)
-- Name: document_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY document_matter
    ADD CONSTRAINT document_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2108 (class 2606 OID 114258)
-- Name: document_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY document
    ADD CONSTRAINT document_pkey PRIMARY KEY (id);


--
-- TOC entry 2112 (class 2606 OID 114260)
-- Name: document_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY document_task
    ADD CONSTRAINT document_task_pkey PRIMARY KEY (id);


--
-- TOC entry 2119 (class 2606 OID 114262)
-- Name: event_assigned_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_assigned_contact
    ADD CONSTRAINT event_assigned_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2121 (class 2606 OID 114264)
-- Name: event_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_matter
    ADD CONSTRAINT event_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2123 (class 2606 OID 114266)
-- Name: event_note_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_note
    ADD CONSTRAINT event_note_pkey PRIMARY KEY (id);


--
-- TOC entry 2117 (class 2606 OID 114268)
-- Name: event_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event
    ADD CONSTRAINT event_pkey PRIMARY KEY (id);


--
-- TOC entry 2125 (class 2606 OID 114270)
-- Name: event_responsible_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_responsible_user
    ADD CONSTRAINT event_responsible_user_pkey PRIMARY KEY (id);


--
-- TOC entry 2127 (class 2606 OID 114272)
-- Name: event_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_tag
    ADD CONSTRAINT event_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 2129 (class 2606 OID 114274)
-- Name: event_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY event_task
    ADD CONSTRAINT event_task_pkey PRIMARY KEY (id);


--
-- TOC entry 2133 (class 2606 OID 114276)
-- Name: matter_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT matter_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2131 (class 2606 OID 114278)
-- Name: matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2135 (class 2606 OID 114280)
-- Name: matter_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT matter_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 2139 (class 2606 OID 114282)
-- Name: note_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT note_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2137 (class 2606 OID 114284)
-- Name: note_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note
    ADD CONSTRAINT note_pkey PRIMARY KEY (id);


--
-- TOC entry 2141 (class 2606 OID 114286)
-- Name: note_task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT note_task_pkey PRIMARY KEY (id);


--
-- TOC entry 2115 (class 2606 OID 114288)
-- Name: pk_elmah_error; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY elmah_error
    ADD CONSTRAINT pk_elmah_error PRIMARY KEY (errorid);


--
-- TOC entry 2143 (class 2606 OID 114290)
-- Name: responsible_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT responsible_user_pkey PRIMARY KEY (id);


--
-- TOC entry 2149 (class 2606 OID 114292)
-- Name: secured_resource_acl_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT secured_resource_acl_pkey PRIMARY KEY (id);


--
-- TOC entry 2145 (class 2606 OID 114294)
-- Name: secured_resource_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT secured_resource_pkey PRIMARY KEY (id);


--
-- TOC entry 2151 (class 2606 OID 114296)
-- Name: tag_category_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT tag_category_pkey PRIMARY KEY (id);


--
-- TOC entry 2154 (class 2606 OID 114298)
-- Name: tag_filter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY tag_filter
    ADD CONSTRAINT tag_filter_pkey PRIMARY KEY (id);


--
-- TOC entry 2158 (class 2606 OID 114300)
-- Name: task_assigned_contact_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT task_assigned_contact_pkey PRIMARY KEY (id);


--
-- TOC entry 2162 (class 2606 OID 114302)
-- Name: task_matter_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT task_matter_pkey PRIMARY KEY (id);


--
-- TOC entry 2156 (class 2606 OID 114304)
-- Name: task_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task
    ADD CONSTRAINT task_pkey PRIMARY KEY (id);


--
-- TOC entry 2164 (class 2606 OID 114306)
-- Name: task_responsible_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT task_responsible_user_pkey PRIMARY KEY (id);


--
-- TOC entry 2166 (class 2606 OID 114308)
-- Name: task_tag_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT task_tag_pkey PRIMARY KEY (id);


--
-- TOC entry 2168 (class 2606 OID 114310)
-- Name: task_time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT task_time_pkey PRIMARY KEY (id);


--
-- TOC entry 2170 (class 2606 OID 114312)
-- Name: time_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT time_pkey PRIMARY KEY (id);


--
-- TOC entry 2174 (class 2606 OID 114314)
-- Name: user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- TOC entry 2176 (class 2606 OID 114316)
-- Name: version_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY version
    ADD CONSTRAINT version_pkey PRIMARY KEY (id);


--
-- TOC entry 2113 (class 1259 OID 114317)
-- Name: ix_elmah_error_app_time_seq; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE INDEX ix_elmah_error_app_time_seq ON elmah_error USING btree (application, timeutc DESC, sequence DESC);


--
-- TOC entry 2100 (class 1259 OID 114318)
-- Name: uidx_area_name; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_area_name ON area USING btree (name);


--
-- TOC entry 2152 (class 1259 OID 114319)
-- Name: uidx_tagcategory_name; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_tagcategory_name ON tag_category USING btree (name);


--
-- TOC entry 2171 (class 1259 OID 114320)
-- Name: uidx_user_userauthtoken; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_user_userauthtoken ON "user" USING btree (user_auth_token);


--
-- TOC entry 2172 (class 1259 OID 114321)
-- Name: uidx_user_username; Type: INDEX; Schema: public; Owner: postgres; Tablespace: 
--

CREATE UNIQUE INDEX uidx_user_username ON "user" USING btree (username);


--
-- TOC entry 2185 (class 2606 OID 114322)
-- Name: FK_area_acl_area_SecurityAreaId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_area_SecurityAreaId" FOREIGN KEY (security_area_id) REFERENCES area(id);


--
-- TOC entry 2184 (class 2606 OID 114327)
-- Name: FK_area_acl_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2183 (class 2606 OID 114332)
-- Name: FK_area_acl_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2182 (class 2606 OID 114337)
-- Name: FK_area_acl_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2181 (class 2606 OID 114342)
-- Name: FK_area_acl_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area_acl
    ADD CONSTRAINT "FK_area_acl_user_UserId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2180 (class 2606 OID 114347)
-- Name: FK_area_area_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_area_ParentId" FOREIGN KEY (parent_id) REFERENCES area(id);


--
-- TOC entry 2179 (class 2606 OID 114352)
-- Name: FK_area_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2178 (class 2606 OID 114357)
-- Name: FK_area_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2177 (class 2606 OID 114362)
-- Name: FK_area_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY area
    ADD CONSTRAINT "FK_area_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2188 (class 2606 OID 114367)
-- Name: FK_contact_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2187 (class 2606 OID 114372)
-- Name: FK_contact_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2186 (class 2606 OID 114377)
-- Name: FK_contact_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY contact
    ADD CONSTRAINT "FK_contact_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2196 (class 2606 OID 114382)
-- Name: FK_document_matter_document_DocumentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_matter
    ADD CONSTRAINT "FK_document_matter_document_DocumentId" FOREIGN KEY (document_id) REFERENCES document(id);


--
-- TOC entry 2195 (class 2606 OID 114387)
-- Name: FK_document_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_matter
    ADD CONSTRAINT "FK_document_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2194 (class 2606 OID 114392)
-- Name: FK_document_matter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_matter
    ADD CONSTRAINT "FK_document_matter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2193 (class 2606 OID 114397)
-- Name: FK_document_matter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_matter
    ADD CONSTRAINT "FK_document_matter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2192 (class 2606 OID 114402)
-- Name: FK_document_matter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_matter
    ADD CONSTRAINT "FK_document_matter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2201 (class 2606 OID 114407)
-- Name: FK_document_task_document_DocumentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_task
    ADD CONSTRAINT "FK_document_task_document_DocumentId" FOREIGN KEY (document_id) REFERENCES document(id);


--
-- TOC entry 2200 (class 2606 OID 114412)
-- Name: FK_document_task_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_task
    ADD CONSTRAINT "FK_document_task_matter_MatterId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2199 (class 2606 OID 114417)
-- Name: FK_document_task_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_task
    ADD CONSTRAINT "FK_document_task_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2198 (class 2606 OID 114422)
-- Name: FK_document_task_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_task
    ADD CONSTRAINT "FK_document_task_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2197 (class 2606 OID 114427)
-- Name: FK_document_task_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document_task
    ADD CONSTRAINT "FK_document_task_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2191 (class 2606 OID 114432)
-- Name: FK_document_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document
    ADD CONSTRAINT "FK_document_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2190 (class 2606 OID 114437)
-- Name: FK_document_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document
    ADD CONSTRAINT "FK_document_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2189 (class 2606 OID 114442)
-- Name: FK_document_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY document
    ADD CONSTRAINT "FK_document_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2209 (class 2606 OID 114447)
-- Name: FK_event_assigned_contact_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_assigned_contact
    ADD CONSTRAINT "FK_event_assigned_contact_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2208 (class 2606 OID 114452)
-- Name: FK_event_assigned_contact_event_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_assigned_contact
    ADD CONSTRAINT "FK_event_assigned_contact_event_EventId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2207 (class 2606 OID 114457)
-- Name: FK_event_assigned_contact_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_assigned_contact
    ADD CONSTRAINT "FK_event_assigned_contact_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2206 (class 2606 OID 114462)
-- Name: FK_event_assigned_contact_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_assigned_contact
    ADD CONSTRAINT "FK_event_assigned_contact_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2205 (class 2606 OID 114467)
-- Name: FK_event_assigned_contact_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_assigned_contact
    ADD CONSTRAINT "FK_event_assigned_contact_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2214 (class 2606 OID 114472)
-- Name: FK_event_matter_event_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_matter
    ADD CONSTRAINT "FK_event_matter_event_EventId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2213 (class 2606 OID 114477)
-- Name: FK_event_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_matter
    ADD CONSTRAINT "FK_event_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2212 (class 2606 OID 114482)
-- Name: FK_event_matter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_matter
    ADD CONSTRAINT "FK_event_matter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2211 (class 2606 OID 114487)
-- Name: FK_event_matter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_matter
    ADD CONSTRAINT "FK_event_matter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2210 (class 2606 OID 114492)
-- Name: FK_event_matter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_matter
    ADD CONSTRAINT "FK_event_matter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2219 (class 2606 OID 114497)
-- Name: FK_event_note_event_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_note
    ADD CONSTRAINT "FK_event_note_event_EventId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2218 (class 2606 OID 114502)
-- Name: FK_event_note_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_note
    ADD CONSTRAINT "FK_event_note_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2217 (class 2606 OID 114507)
-- Name: FK_event_note_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_note
    ADD CONSTRAINT "FK_event_note_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2216 (class 2606 OID 114512)
-- Name: FK_event_note_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_note
    ADD CONSTRAINT "FK_event_note_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2215 (class 2606 OID 114517)
-- Name: FK_event_note_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_note
    ADD CONSTRAINT "FK_event_note_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2224 (class 2606 OID 114522)
-- Name: FK_event_responsible_user_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_responsible_user
    ADD CONSTRAINT "FK_event_responsible_user_task_TaskId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2223 (class 2606 OID 114527)
-- Name: FK_event_responsible_user_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_responsible_user
    ADD CONSTRAINT "FK_event_responsible_user_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2222 (class 2606 OID 114532)
-- Name: FK_event_responsible_user_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_responsible_user
    ADD CONSTRAINT "FK_event_responsible_user_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2221 (class 2606 OID 114537)
-- Name: FK_event_responsible_user_user_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_responsible_user
    ADD CONSTRAINT "FK_event_responsible_user_user_MatterId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2220 (class 2606 OID 114542)
-- Name: FK_event_responsible_user_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_responsible_user
    ADD CONSTRAINT "FK_event_responsible_user_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2229 (class 2606 OID 114547)
-- Name: FK_event_tag_event_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_tag
    ADD CONSTRAINT "FK_event_tag_event_EventId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2228 (class 2606 OID 114552)
-- Name: FK_event_tag_tag_category_TagCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_tag
    ADD CONSTRAINT "FK_event_tag_tag_category_TagCategoryId" FOREIGN KEY (tag_category_id) REFERENCES tag_category(id);


--
-- TOC entry 2227 (class 2606 OID 114557)
-- Name: FK_event_tag_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_tag
    ADD CONSTRAINT "FK_event_tag_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2226 (class 2606 OID 114562)
-- Name: FK_event_tag_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_tag
    ADD CONSTRAINT "FK_event_tag_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2225 (class 2606 OID 114567)
-- Name: FK_event_tag_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_tag
    ADD CONSTRAINT "FK_event_tag_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2234 (class 2606 OID 114572)
-- Name: FK_event_task_event_EventId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_task
    ADD CONSTRAINT "FK_event_task_event_EventId" FOREIGN KEY (event_id) REFERENCES event(id);


--
-- TOC entry 2233 (class 2606 OID 114577)
-- Name: FK_event_task_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_task
    ADD CONSTRAINT "FK_event_task_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2232 (class 2606 OID 114582)
-- Name: FK_event_task_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_task
    ADD CONSTRAINT "FK_event_task_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2231 (class 2606 OID 114587)
-- Name: FK_event_task_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_task
    ADD CONSTRAINT "FK_event_task_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2230 (class 2606 OID 114592)
-- Name: FK_event_task_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event_task
    ADD CONSTRAINT "FK_event_task_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2204 (class 2606 OID 114597)
-- Name: FK_event_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event
    ADD CONSTRAINT "FK_event_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2203 (class 2606 OID 114602)
-- Name: FK_event_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event
    ADD CONSTRAINT "FK_event_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2202 (class 2606 OID 114607)
-- Name: FK_event_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY event
    ADD CONSTRAINT "FK_event_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2243 (class 2606 OID 114612)
-- Name: FK_matter_contact_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2242 (class 2606 OID 114617)
-- Name: FK_matter_contact_user_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2241 (class 2606 OID 114622)
-- Name: FK_matter_contact_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2240 (class 2606 OID 114627)
-- Name: FK_matter_contact_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2239 (class 2606 OID 114632)
-- Name: FK_matter_contact_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_contact
    ADD CONSTRAINT "FK_matter_contact_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2238 (class 2606 OID 114637)
-- Name: FK_matter_matter_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_matter_ParentId" FOREIGN KEY (parent_id) REFERENCES matter(id);


--
-- TOC entry 2248 (class 2606 OID 114642)
-- Name: FK_matter_tag_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2247 (class 2606 OID 114647)
-- Name: FK_matter_tag_tag_category_TagCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_tag_category_TagCategoryId" FOREIGN KEY (tag_category_id) REFERENCES tag_category(id);


--
-- TOC entry 2246 (class 2606 OID 114652)
-- Name: FK_matter_tag_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2245 (class 2606 OID 114657)
-- Name: FK_matter_tag_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2244 (class 2606 OID 114662)
-- Name: FK_matter_tag_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter_tag
    ADD CONSTRAINT "FK_matter_tag_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2237 (class 2606 OID 114667)
-- Name: FK_matter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2236 (class 2606 OID 114672)
-- Name: FK_matter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2235 (class 2606 OID 114677)
-- Name: FK_matter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY matter
    ADD CONSTRAINT "FK_matter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2256 (class 2606 OID 114682)
-- Name: FK_note_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2255 (class 2606 OID 114687)
-- Name: FK_note_matter_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2254 (class 2606 OID 114692)
-- Name: FK_note_matter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2253 (class 2606 OID 114697)
-- Name: FK_note_matter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2252 (class 2606 OID 114702)
-- Name: FK_note_matter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_matter
    ADD CONSTRAINT "FK_note_matter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2261 (class 2606 OID 114707)
-- Name: FK_note_task_note_NoteId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_note_NoteId" FOREIGN KEY (note_id) REFERENCES note(id);


--
-- TOC entry 2260 (class 2606 OID 114712)
-- Name: FK_note_task_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2259 (class 2606 OID 114717)
-- Name: FK_note_task_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2258 (class 2606 OID 114722)
-- Name: FK_note_task_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2257 (class 2606 OID 114727)
-- Name: FK_note_task_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note_task
    ADD CONSTRAINT "FK_note_task_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2251 (class 2606 OID 114732)
-- Name: FK_note_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note
    ADD CONSTRAINT "FK_note_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2250 (class 2606 OID 114737)
-- Name: FK_note_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note
    ADD CONSTRAINT "FK_note_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2249 (class 2606 OID 114742)
-- Name: FK_note_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY note
    ADD CONSTRAINT "FK_note_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2266 (class 2606 OID 114747)
-- Name: FK_responsible_user_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2265 (class 2606 OID 114752)
-- Name: FK_responsible_user_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2264 (class 2606 OID 114757)
-- Name: FK_responsible_user_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2263 (class 2606 OID 114762)
-- Name: FK_responsible_user_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2262 (class 2606 OID 114767)
-- Name: FK_responsible_user_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY responsible_user
    ADD CONSTRAINT "FK_responsible_user_user_UserId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2274 (class 2606 OID 114772)
-- Name: FK_secured_resource_acl_secured_resource_SecuredResourceId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_secured_resource_SecuredResourceId" FOREIGN KEY (secured_resource_id) REFERENCES secured_resource(id);


--
-- TOC entry 2273 (class 2606 OID 114777)
-- Name: FK_secured_resource_acl_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2272 (class 2606 OID 114782)
-- Name: FK_secured_resource_acl_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2271 (class 2606 OID 114787)
-- Name: FK_secured_resource_acl_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2270 (class 2606 OID 114792)
-- Name: FK_secured_resource_acl_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource_acl
    ADD CONSTRAINT "FK_secured_resource_acl_user_UserId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2269 (class 2606 OID 114797)
-- Name: FK_secured_resource_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT "FK_secured_resource_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2268 (class 2606 OID 114802)
-- Name: FK_secured_resource_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT "FK_secured_resource_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2267 (class 2606 OID 114807)
-- Name: FK_secured_resource_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY secured_resource
    ADD CONSTRAINT "FK_secured_resource_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2277 (class 2606 OID 114812)
-- Name: FK_tag_category_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT "FK_tag_category_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2276 (class 2606 OID 114817)
-- Name: FK_tag_category_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT "FK_tag_category_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2275 (class 2606 OID 114822)
-- Name: FK_tag_category_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_category
    ADD CONSTRAINT "FK_tag_category_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2281 (class 2606 OID 114827)
-- Name: FK_tag_filter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_filter
    ADD CONSTRAINT "FK_tag_filter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2280 (class 2606 OID 114832)
-- Name: FK_tag_filter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_filter
    ADD CONSTRAINT "FK_tag_filter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2279 (class 2606 OID 114837)
-- Name: FK_tag_filter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_filter
    ADD CONSTRAINT "FK_tag_filter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2278 (class 2606 OID 114842)
-- Name: FK_tag_filter_user_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY tag_filter
    ADD CONSTRAINT "FK_tag_filter_user_UserId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2291 (class 2606 OID 114847)
-- Name: FK_task_assigned_contact_contact_ContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_contact_ContactId" FOREIGN KEY (contact_id) REFERENCES contact(id);


--
-- TOC entry 2290 (class 2606 OID 114852)
-- Name: FK_task_assigned_contact_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2289 (class 2606 OID 114857)
-- Name: FK_task_assigned_contact_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2288 (class 2606 OID 114862)
-- Name: FK_task_assigned_contact_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2287 (class 2606 OID 114867)
-- Name: FK_task_assigned_contact_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_assigned_contact
    ADD CONSTRAINT "FK_task_assigned_contact_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2296 (class 2606 OID 114872)
-- Name: FK_task_matter_matter_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_matter_MatterId" FOREIGN KEY (matter_id) REFERENCES matter(id);


--
-- TOC entry 2295 (class 2606 OID 114877)
-- Name: FK_task_matter_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2294 (class 2606 OID 114882)
-- Name: FK_task_matter_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2293 (class 2606 OID 114887)
-- Name: FK_task_matter_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2292 (class 2606 OID 114892)
-- Name: FK_task_matter_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_matter
    ADD CONSTRAINT "FK_task_matter_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2301 (class 2606 OID 114897)
-- Name: FK_task_responsible_user_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2300 (class 2606 OID 114902)
-- Name: FK_task_responsible_user_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2299 (class 2606 OID 114907)
-- Name: FK_task_responsible_user_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2298 (class 2606 OID 114912)
-- Name: FK_task_responsible_user_user_MatterId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_MatterId" FOREIGN KEY (user_id) REFERENCES "user"(id);


--
-- TOC entry 2297 (class 2606 OID 114917)
-- Name: FK_task_responsible_user_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_responsible_user
    ADD CONSTRAINT "FK_task_responsible_user_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2306 (class 2606 OID 114922)
-- Name: FK_task_tag_tag_category_TagCategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_tag_category_TagCategoryId" FOREIGN KEY (tag_category_id) REFERENCES tag_category(id);


--
-- TOC entry 2305 (class 2606 OID 114927)
-- Name: FK_task_tag_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2304 (class 2606 OID 114932)
-- Name: FK_task_tag_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2303 (class 2606 OID 114937)
-- Name: FK_task_tag_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2302 (class 2606 OID 114942)
-- Name: FK_task_tag_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_tag
    ADD CONSTRAINT "FK_task_tag_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2286 (class 2606 OID 115115)
-- Name: FK_task_task_ParentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_ParentId" FOREIGN KEY (parent_id) REFERENCES task(id);


--
-- TOC entry 2285 (class 2606 OID 115120)
-- Name: FK_task_task_SequentialPredecessorId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_task_SequentialPredecessorId" FOREIGN KEY (sequential_predecessor_id) REFERENCES task(id);


--
-- TOC entry 2311 (class 2606 OID 114957)
-- Name: FK_task_time_task_TaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_task_TaskId" FOREIGN KEY (task_id) REFERENCES task(id);


--
-- TOC entry 2310 (class 2606 OID 114962)
-- Name: FK_task_time_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2309 (class 2606 OID 114967)
-- Name: FK_task_time_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2308 (class 2606 OID 114972)
-- Name: FK_task_time_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2307 (class 2606 OID 114977)
-- Name: FK_task_time_user_TimeId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task_time
    ADD CONSTRAINT "FK_task_time_user_TimeId" FOREIGN KEY (time_id) REFERENCES "time"(id);


--
-- TOC entry 2284 (class 2606 OID 115125)
-- Name: FK_task_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2283 (class 2606 OID 115130)
-- Name: FK_task_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2282 (class 2606 OID 115135)
-- Name: FK_task_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY task
    ADD CONSTRAINT "FK_task_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2315 (class 2606 OID 115043)
-- Name: FK_time_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2314 (class 2606 OID 115048)
-- Name: FK_time_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2313 (class 2606 OID 115053)
-- Name: FK_time_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2312 (class 2606 OID 115058)
-- Name: FK_time_user_WorkerContactId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY "time"
    ADD CONSTRAINT "FK_time_user_WorkerContactId" FOREIGN KEY (worker_contact_id) REFERENCES contact(id);


--
-- TOC entry 2318 (class 2606 OID 115017)
-- Name: FK_version_user_CreatedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY version
    ADD CONSTRAINT "FK_version_user_CreatedByUserId" FOREIGN KEY (created_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2317 (class 2606 OID 115022)
-- Name: FK_version_user_DisabledByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY version
    ADD CONSTRAINT "FK_version_user_DisabledByUserId" FOREIGN KEY (disabled_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2316 (class 2606 OID 115027)
-- Name: FK_version_user_ModifiedByUserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY version
    ADD CONSTRAINT "FK_version_user_ModifiedByUserId" FOREIGN KEY (modified_by_user_id) REFERENCES "user"(id);


--
-- TOC entry 2325 (class 0 OID 0)
-- Dependencies: 6
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2014-06-13 20:11:58

--
-- PostgreSQL database dump complete
--

